using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;
using StudioConcept.MVVM;
using MediaColor = System.Windows.Media.Color;

namespace StudioConcept.Observer
{
    public class ObserverService
    {
        public MediaColor Transparent = MediaColor.FromArgb(0, 0, 0, 0);
        public MediaColor WarningColor = MediaColor.FromArgb(255, 0, 0, 0);
        public static ObservableCollection<BaseShape> Parent;
        public static IInputElement MileStone;
        private readonly BaseShape _pivot;
        // this guy only for interact bottom/above.
        private BaseShape _previousShape;
        // save state
        private State _previousState;
        private State _currentState;
        // this guy for interact middle, parent/children relations.
        private BaseShape _parentShape;
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

            //prioritize for middle->bottom->above
            Watch();
            // current is stateless, we revert the change of previous state.
            if (_currentState.Stateless())
            {
                if (!_previousState.Stateless())
                {
                    _previousState.InteractShape.DoAll(i=>i.Redo());
                }
            }
            else
            {
                // if previous is stateless.
                if (_previousState.Stateless())
                {
                    _previousState = _currentState;
                    
                    WarningEventHandler?.Invoke(this, _currentState.InteractShape);
                    _currentState.InteractShape.DoAllWithoutHead(i => i.Down(_pivot.Height + 8.8));
                }
                else
                {
                    Log(_currentState.InteractShape, "current");
                    Log(_previousState.InteractShape, "_previousState");
                    if (!_currentState.Equal(_previousState))
                    {
                        Log(_currentState.InteractShape, "current");
                        Log(_previousState.InteractShape, "_previousState");
                        // in same dll
                        if (_currentState.InteractShape.GetHead()==_previousState.InteractShape.GetHead())
                        {
                            if (_currentState.InteractShape.Next==_previousState.InteractShape)
                            {
                                
                                _currentState.InteractShape.Redo();
                            }
                            else
                            {
                                _currentState.InteractShape.DoAll(i=>i.Down(_pivot.Height+8.8));
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// When release mouse, we invoke the binding event, then remove all other attribute.
        /// </summary>
        public void Magnet()
        {
            InteractEventHandler?.Invoke(this, _previousState.InteractShape);
            PregnantEventHandler?.Invoke(this, _parentShape);
            InteractEventHandler = null;
            PregnantEventHandler = null;
            WarningEventHandler = null;
            WarningExpandEventHandler = null;
            _previousShape = null;
            _parentShape = null;
            
        }

        private void MagnetBot(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }

           
            //determine relationship for pivot and its new head.
            if (other.IsTail())
            {
                //other is a single
                //calculate the position of pivot.
                _pivot.X = other.X;
                var delta = Math.Abs(other.InnerLowerY + 8.8 - _pivot.Y);
                _pivot.Y = other.InnerLowerY + 8.8;
                //calculate the next shape head by pivot.
                _pivot.DoAllWithoutHead(i =>
                {
                    i.X = other.X;
                    i.Y -= delta;
                });
            }
            else
            {
                // when drag pivot in middle of two shape

                //calculate the position of pivot.
                _pivot.X = other.X;
                var delta = Math.Abs(other.InnerLowerY + 8.8 - _pivot.Y);
                _pivot.Y = other.InnerLowerY + 8.8;
                
                double totalHeight = 0;
                _pivot.DoAllWithoutHead(i =>
                {
                    i.X = other.X;
                    i.Y -= delta;
                    totalHeight += i.Height + 8.8;
                });
                other.Next.Y += totalHeight;

                //calculate the next shape head by pivot.
                var next = other.Next;
                next.Prev = _pivot.GetTail();
                _pivot.GetTail().Next = next;
                
            }
            other.Next = _pivot;
            _pivot.Prev = other;
            _pivot.StrokeColor = Transparent;
            other.StrokeColor = Transparent;
            _previousState.Clear();
        }

        private void MagnetTop(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }
            //calculate the position of pivot.
            _pivot.X = other.X;
            var delta = Math.Abs(other.InnerLowerY + 8.8 - _pivot.Y);
            _pivot.Y = other.Y - _pivot.Height - 8.8;
            //calculate the next shape head by pivot.
            _pivot.DoAllWithoutHead(i =>
            {
                i.X = other.X;
                i.Y += delta;
            });
            //determine relationship for pivot and its new head.
            _pivot.Next = other;
            other.Prev = _pivot;
            _pivot.StrokeColor = Transparent;
            other.StrokeColor = Transparent;
            _previousState.Clear();
        }

        // only for IBranch
        private void MagnetMiddle(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }

            _pivot.Parent = other;
            if (!other.ChildrenNode.Contains(_pivot))
            {
                other.ChildrenNode.Add(_pivot);
                
            }
            _pivot.X = other.X + 16;
            _pivot.Y = ((IfShape) other).MiddleUpperY;

            _pivot.StrokeColor = Transparent;
            other.StrokeColor = Transparent;

        }

        private void Pregnant(object source, BaseShape other)
        {
            if (other==null)
            {
                return;
            }

            if (!other.ChildrenNode.Contains(_pivot))
            {
                _pivot.Parent = other;
                other.ChildrenNode.Add(_pivot);
            }
        }

        private void RemoveRelation()
        {
            var temp = _pivot.Prev;
            _pivot.Prev = null;
            if (temp!=null)
            {
                temp.Next = null;
            }
        }

        private void LightBorder(object source, BaseShape other)
        {
            _pivot.StrokeColor = WarningColor;
            other.StrokeColor = WarningColor;
        }

        private void LightBorderAndPush(bool needPushAll, BaseShape other)
        {
            if (needPushAll)
            {
                other.Y += _pivot.Height + 8.8;
            }
            
            other.DoAllWithoutHead(i=>i.Y+= _pivot.Height + 8.8);
        }

        private void LightBorderAndExpand(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }
            _pivot.StrokeColor = WarningColor;
            other.StrokeColor = WarningColor;

           
            if (other is IBranch)
            {
                if (((IfShape)other).ChildrenNode.Count==0)
                {
                    ((IfShape) other).MiddleSpace = _pivot.Height;
                }
                else
                {
                    ((IfShape) other).MiddleSpace += _pivot.Height+8.8;
                }
            }
        }

        private void RemoveLightBorder()
        {
            _pivot.StrokeColor = Transparent;
            _previousShape.StrokeColor = Transparent;
        }
        /// <summary>
        /// what do our pivot interact with.
        /// </summary>
        /// <returns></returns>
        private void Watch()
        {
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
                // ignore descendants.
                if (_pivot.ChildrenNode.Contains(i))
                {
                    return false;
                }

                if (_pivot.Parent==i)
                {
                    return false;
                }
                return true;
            }).ToList();

            //prioritize : middle -> bottom -> above.
            var temp = interactSet.FirstOrDefault(i =>
            {
                // todo consider if got more than 1 block.
                if (_pivot.IsAtMiddle(i))
                {
                    InteractEventHandler = null;
                    WarningExpandEventHandler = null;
                    PregnantEventHandler += MagnetMiddle;
                    WarningExpandEventHandler += LightBorderAndExpand;
                    _currentState.InteractDirection = ShapeState.Middle;
                    return true;
                }

                // drag to bottom of other
                if (_pivot.IsAtBottom(i))
                {
                    InteractEventHandler = null;
                    WarningEventHandler = null;
                    InteractEventHandler += MagnetBot;
                    WarningEventHandler += LightBorder;
                    _currentState.InteractDirection = ShapeState.Down;
                    return true;
                }
                // drag to above of other
                if (_pivot.IsAtAbove(i))
                {
                    InteractEventHandler = null;
                    WarningEventHandler = null;
                    InteractEventHandler += MagnetTop;
                    _currentState.InteractDirection = ShapeState.Up;
                    WarningEventHandler += LightBorder;

                    return true;
                }
                return false;
            });
            _currentState.InteractShape = temp;
        }

        private void Log(BaseShape temp, string prefix)
        {
            Console.WriteLine("---------------------------------");
            Console.WriteLine(prefix+" :" +temp?.Text);
            Console.WriteLine(prefix + " Relationship: ");
            Console.WriteLine(prefix + " PARENT: " +temp?.Parent?.Text);
            Console.WriteLine(prefix + " CHILD: " +temp?.ChildrenNode.Count);
            Console.WriteLine(prefix + " NEXT: " +temp?.Next?.Text);
            Console.WriteLine(prefix + " PREV: " +temp?.Prev?.Text);
            Console.WriteLine(prefix + " STATE: " +temp?.State);
            Console.WriteLine("---------------------------------");
        }
    }
}
