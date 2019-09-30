using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using StudioConcept.MVVM;
using StudioConcept.Properties;
using MediaColor = System.Windows.Media.Color;

namespace StudioConcept.Observer
{
    public class ObserverService
    {
        public MediaColor WarningColor = MediaColor.FromArgb(255, 0, 0, 0);
        public static ObservableCollection<BaseShape> Parent;
        public static IInputElement MileStone;
        private readonly BaseShape _pivot;
        // this guy only for interact bottom/above.
        // save state
        private State _previousState;
        private readonly State _currentState;
        // this guy for interact middle, parent/children relations.
        private BaseShape _currentParentShape;
        private BaseShape _previousParentShape;
        // this event handle for interact middle.
        public EventHandler<BaseShape> PregnantEventHandler;
        // this event handle only for interact top/bottom.
        public EventHandler<BaseShape> InteractEventHandler;
        // this event handle for warning.
        public EventHandler<BaseShape> WarningEventHandler;
        // this event handle for warning  & expand.
        public EventHandler<BaseShape> WarningExpandEventHandler;
        public ObserverService(BaseShape pivot)
        {
            _pivot = pivot;
            _previousState = new State();
            _currentState = new State();
        }

        public void Tracking()
        {
            if (Parent.Count < 2)
            {
                return;
            }
            // remove itself and on the same linked list.
            var interactSet = Parent.Where(i =>
            {
                // ignore itself.
                if (i == _pivot)
                {
                    return false;
                }
                // ignore member.
                if (_pivot.HeadOf(i))
                {
                    return false;
                }
                if (_pivot.DoAll(ele => ele.HasDescendents(i)))
                {
                    return false;
                }
                // ignore ancestor
                if (_pivot.IsDescendents(i))
                {
                    return false;
                }
                return true;
            }).ToList();
            
            //prioritize for middle->bottom->above
            Watch(interactSet);
            // current is stateless, we revert the change of previous state.
            if (_currentState.Stateless())
            {
                _previousState.InteractShape?.DoAll(i => i.Redo());
                _previousParentShape?.UpdateMiddleSpace();
                _previousParentShape = null;
                var prev = _pivot.Prev;
                _pivot.RemoveHead();
                _pivot.DoAll(i=>i.Alone());
                prev?.DoInverseAll(i => i.VirtualHeight = i.ReCalculateVirtualHeight());
                _currentParentShape = null;
            }

            if (_currentState.InteractDirection!=State.ShapeState.Middle)
            {
                _previousParentShape?.UpdateMiddleSpace();
            }
            
            // interact with control block
            if (!_currentState.Stateless() &&_currentState.InteractDirection == State.ShapeState.Middle)
            {
                if (_currentState.InteractShape!= _previousParentShape)
                {
                    _previousParentShape?.UpdateMiddleSpace();
                }
                _currentParentShape = _currentState.InteractShape;
                interactSet = _currentParentShape.ChildrenNode.ToList();
                _currentParentShape.Expand(_pivot.Height + 8.8);
                _previousParentShape = _currentParentShape;
                Watch(interactSet);
            }
            ProcessInLinear();
        }

        /// <summary>
        /// When release mouse, we invoke the binding event, then remove all other attribute.
        /// </summary>
        public void Magnet()
        {
            InteractEventHandler?.Invoke(this, _currentState.InteractShape);
            PregnantEventHandler?.Invoke(this, _currentParentShape);
            _currentState.Clear();
            _previousState.Clear();
            foreach (var baseShape in Parent)
            {
                baseShape.State = State.ShapeState.Balance;
            }
            InteractEventHandler = null;
            PregnantEventHandler = null;
            WarningEventHandler = null;
            WarningExpandEventHandler = null;
            WriteLog();
        }

        private void MagnetBot(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }

            if (other.Next != null && other.Next.Equals(_pivot))
            {
                return;
            }
           
            //determine relationship for pivot and its new head.
            if (other.IsTail())
            {
                //other is a single
                //calculate the position of pivot.
                _pivot.SetCoordinate(valueX:other.X, valueY: other.VirtualInnerLowerY + 8.8);
                other.InsertTail(_pivot);
            }
            else
            {
                // when drag pivot in middle of two shape
                //calculate the position of pivot.
                _pivot.SetCoordinate(valueX:other.X, valueY: other.Y + other.Height + 8.8);

                var next = other.Next;
                // ReSharper disable once PossibleNullReferenceException
                next.DoAll(i=>i.Redo());
                next.DoAll(i=>i.Y += _pivot.VirtualHeight + 8.8*_pivot.Count);
                _pivot.InsertTail(next);
                other.Next = null;
                other.InsertTail(_pivot);
            }

            _pivot.ResetStroke();
            other.ResetStroke();
            _previousState.Clear();
        }

        private void MagnetTop(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }
            //calculate the position of pivot.
            other.SetCoordinate(valueX:_pivot.X, valueY: _pivot.VirtualInnerLowerY + 8.8 * _pivot.Count);
            //determine relationship for pivot and its new head.
            _pivot.InsertTail(other);
            _pivot.ResetStroke();
            other.ResetStroke();
            _previousState.Clear();
        }

        // only for IBranch
        private void MagnetMiddle(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }
            // change logic for each child.
            _pivot.DoAll(i =>
            {
                if (!other.ChildrenNode.Contains(i))
                {
                    other.ChildrenNode.Add(i);
                    i.Parent = other;
                }
            });

            other.ArrangeChildrenPosition();

            _pivot.ResetStroke();
            other.ResetStroke();

        }

        private void LightBorder(object source, BaseShape other)
        {
            _pivot.StrokeColor = WarningColor;
            other.StrokeColor = WarningColor;
        }

        
        /// <summary>
        /// what do our pivot interact with.
        /// TODO upgrade it, and tune if have time
        /// </summary>
        /// <returns></returns>
        private void Watch(List<BaseShape> interactSet)
        {
            State.Copy(_currentState, ref _previousState);
            // firstly, i filter the eligible.
            var filter = interactSet.Where(i =>
                _pivot.IsAtMiddle(i) || _pivot.IsAtBottom(i) || _pivot.IsAtAbove(i)).ToList();
            if (filter.Count==0)
            {
                _currentState.InteractDirection = null;
                _currentState.InteractShape = null;
                return;
            }
            // after this filter, just have 3 case: bottom: 1 block, above: 1 block, middle: n block.
            var middles = filter.Where(i =>
            {
                if (_pivot.IsAtMiddle(i))
                {
                    return true;
                }

                return false;
            }).OrderByDescending(i=>i.Y).FirstOrDefault(); 
            if (middles!=null)
            {
                
                InteractEventHandler = null;
                WarningExpandEventHandler = null;
                PregnantEventHandler += MagnetMiddle;
                _currentState.InteractDirection = State.ShapeState.Middle;
                _currentState.InteractShape = middles;

            }
            else
            {
                var down = filter.FirstOrDefault(i => _pivot.IsAtBottom(i));
                if (down != null)
                {
                    InteractEventHandler = null;
                    WarningEventHandler = null;
                    InteractEventHandler += MagnetBot;
                    WarningEventHandler += LightBorder;
                    _currentState.InteractDirection = State.ShapeState.Down;
                    _currentState.InteractShape = down;

                }
                else
                {
                    var up = filter.FirstOrDefault(i => _pivot.IsAtAbove(i));
                    if (up!=null)
                    {
                        InteractEventHandler = null;
                        WarningEventHandler = null;
                        InteractEventHandler += MagnetTop;
                        _currentState.InteractDirection = State.ShapeState.Changed;
                        WarningEventHandler += LightBorder;
                        _currentState.InteractShape = up;

                    }
                    else
                    {
                        _currentState.InteractDirection = null;
                        _currentState.InteractShape = null;
                    }
                }
            }
            
            
        }

        private void ProcessInLinear()
        {
            // redo anything previous.
            if (_currentState.Stateless())
            {
                if (!_previousState.Stateless())
                {
                    _previousState.InteractShape.DoAll(i => i.Redo());
                }
            }
            // current is stateless, we revert the change of previous state.
            else
            {
                // if previous is stateless.
                if (_previousState.Stateless())
                {
                    WarningEventHandler?.Invoke(this, _currentState.InteractShape);
                    if (_currentState.InteractDirection==State.ShapeState.Down)
                    {
                        _currentState.InteractShape.Redo();
                        _currentState.InteractShape.DoAllWithoutHead(i => i.Down(_pivot.Height + 8.8));
                    }else if (_currentState.InteractDirection==State.ShapeState.Changed)
                    {
                        if (_currentState.InteractShape.Parent!=null)
                        {
                            _currentState.InteractShape.DoAll(i => i.Down(_pivot.Height + 8.8));
                        }
                    }
                }
                else
                {
                    //_previousState.InteractShape.DoAll(i => i.Redo());
                    if (!_currentState.Equal(_previousState))
                    {
                        _currentParentShape?.UpdateMiddleSpace();
                        // in same dll
                        if (_currentState.InteractShape.GetHead() == _previousState.InteractShape.GetHead())
                        {
                            if (_currentState.InteractDirection==State.ShapeState.Down)
                            {
                                _currentState.InteractShape.Redo();
                                _currentState.InteractShape.DoAllWithoutHead(i=> i.Down(_pivot.Height + 8.8));
                            }
                            else if (_currentState.InteractDirection == State.ShapeState.Changed)
                            {
                                if (_currentState.InteractShape.Parent!=null)
                                {
                                    _currentState.InteractShape.DoAll(i => i.Down(_pivot.Height + 8.8));
                                }
                            }
                        }
                        // if drag from one to other.
                        else
                        {
                            if (_currentState.InteractShape.IsHead()&&_currentState.InteractShape.Parent!=null)
                            {
                                if (_currentState.InteractDirection==State.ShapeState.Changed)
                                {
                                    _currentState.InteractShape.DoAll(i => i.Down(_pivot.Height + 8.8));
                                }
                                else
                                {
                                    _currentState.InteractShape.DoAllWithoutHead(i => i.Down(_pivot.Height + 8.8));
                                }
                            }
                            else
                            {
                                _currentState.InteractShape.DoAllWithoutHead(i => i.Down(_pivot.Height + 8.8));
                            }
                        }
                    }
                }
            }
        }

        #region Log

        private void WriteLog()
        {
            Console.WriteLine(Resources.ObserverService_WriteLog________________LOG______________);
            foreach (var baseShape in Parent.OrderBy(i => i.Y).ToArray())
            {
                Log(baseShape);
            }
            Console.WriteLine(Resources.ObserverService_WriteLog________________END_LOG______________);
        }

        private void Log(BaseShape shape)
        {
            var prev = shape.Prev == null ? "NO Prev" : shape.Prev?.Text;
            var next = shape.Next == null ? "NO Next" : shape.Next?.Text;
            var parent = shape.Parent == null ? "No Parent" : shape.Parent?.Text;

            Console.WriteLine(shape.Text);
            Console.WriteLine(@"=> Current State: "+shape.State);
            Console.WriteLine(@"=> Middle Space: "+shape.MiddleSpace);
            Console.WriteLine(@"=> Virtual Height: "+shape.VirtualHeight);
            Console.WriteLine(@"=> Count LL: "+shape.Count);
            Console.WriteLine(Resources.ObserverService_Log_ + parent);
            if (shape.ChildrenNode.Count > 0)
            {
                Console.WriteLine(@"=> Children count: " + shape.ChildrenNode.Count);
            }
            Console.WriteLine(@"=> Prev: " + prev);
            Console.WriteLine(@"=> Next: " + next);
        }

        #endregion
    }
}
