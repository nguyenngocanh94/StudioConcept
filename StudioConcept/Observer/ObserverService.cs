using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using StudioConcept.MVVM;
using MediaColor = System.Windows.Media.Color;

namespace StudioConcept.Observer
{
    public class ObserverService
    {
        public MediaColor Transparent = MediaColor.FromArgb(0, 0, 0, 0);
        public static ObservableCollection<BaseShape> Parent;
        public static IInputElement MileStone;
        private readonly BaseShape _pivot;
        private BaseShape _otherShape;
        private BaseShape _parentShape;
        public EventHandler<BaseShape> PregnantEventHandler;
        public EventHandler<BaseShape> InteractEventHandler;
        public EventHandler<BaseShape> WarningEventHandler;
        public EventHandler<BaseShape> WarningExpandEventHandler;
        public ObserverService(BaseShape pivot)
        {
            _pivot = pivot;
        }

        public void Tracking()
        {
            if (Parent.Count < 2)
            {
                return;
            }
            // remove itself and its child.
            // you are dummy
            var interactSet = Parent.Where(i =>
            {
                if (i == _pivot)
                {
                    return false;
                }

                if (_pivot.Contain(i))
                {
                    return false;
                }

                return true;
            }).ToList();

            //prioritize for drag to bottom
            var temp = Watch();
            
            if (temp!=null)
            {
                // interact with IBranch and the type is drag to middle.
                if (temp is IBranch && InteractEventHandler==MagnetMiddle)
                {
                    PregnantEventHandler = null;
                    PregnantEventHandler += Pregnant;
                    if (temp == _parentShape && _parentShape != null||_parentShape!=null)
                    {
                        WarningExpandEventHandler = null;
                    }
                    _parentShape = temp;
                    // watch the interact list if its children node.
                    var interactList = temp.ChildrenNode;
                     
                    var child = interactList.FirstOrDefault(i =>
                    {
                        if (Math.Abs(_pivot.X - i.X) < 40 && _pivot.OuterUpperY < i.OuterLowerY && _pivot.OuterUpperY > i.Y)
                        {
                            WarningEventHandler = null;
                            WarningEventHandler += LightBorder;
                            InteractEventHandler = null;
                            InteractEventHandler += MagnetBot;
                            return true;
                        }
                        // drag to above of other
                        if (Math.Abs(_pivot.X - i.X) < 40 && _pivot.OuterLowerY > i.OuterUpperY && _pivot.Y < i.Y)
                        {
                            InteractEventHandler = null;
                            WarningEventHandler = null;
                            WarningEventHandler += LightBorder;
                            InteractEventHandler += MagnetTop;
                            return true;
                        }

                        return false;
                    });

                    if (child!=null)
                    {
                        if (InteractEventHandler == MagnetTop && child!=_otherShape)
                        {
                            LightBorderAndPush(this, child);
                        }
                        temp = child;
                    }
                    
                    
                }
                // second time, if the next interact is different, remove old warning.
                else if (temp!=_otherShape&&_otherShape!=null)
                {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
                    // if the old and new is same in linked list
                    if (temp.GetHead()==_otherShape.GetHead())
                    {
                        // if interact is not tail.
                        if (!temp.IsTail())
                        {
                            // and not head
                            if (!temp.IsHead())
                            {
                                // expand the tail.
                                temp.DoAllWithoutHead(i => i.Y += _pivot.Height + 8.8);
                            }
                            else
                            {
                                // if drag pivot to bottom of a head, expand the tail
                                if (temp.Y < _pivot.Y)
                                {
                                    temp.DoAllWithoutHead(i => i.Y += _pivot.Height + 8.8);
                                }
                                // else drag to top of a head, do nothing
                            }
                        }
                        _otherShape.DoAllWithoutHead(i=>i.Y-=_pivot.Height+8.8);
                    }
                    else
                    {
                        // else old and new is different linked list, undo expand of old linked list
                        _otherShape.DoAllWithoutHead(i => i.Y -= _pivot.Height+8.8);

                    }

                }
                // first time, just warning.
                WarningEventHandler?.Invoke(this, temp);
                
                WarningExpandEventHandler?.Invoke(this, _parentShape);
                _otherShape = temp;
            }
            else
            {
                // if not see any interact. remove relationship of old interact.
                // clear the warning, undo the expand. update the interact.
                InteractEventHandler = null;
                RemoveRelation();
                _pivot.Parent?.ChildrenNode.Remove(_pivot);
                _pivot.Parent?.UpdateMiddleSpace();
                _parentShape?.UpdateMiddleSpace();
                _pivot.Parent = null;
                _parentShape = null;
                _pivot.StrokeColor = Transparent;
                if (_otherShape!=null)
                {
                    if (!_otherShape.IsTail())
                    {
                        _otherShape.DoAllWithoutHead(i => { i.Y -= 40;});
                    }
                    foreach (var baseShape in _otherShape.ChildrenNode)
                    {
                        baseShape.StrokeColor = Transparent;
                    }
                    _otherShape.StrokeColor = Transparent;
                    _otherShape = null;
                }
            }

        }

        public void Magnet()
        {
            InteractEventHandler?.Invoke(this, _otherShape);
            PregnantEventHandler?.Invoke(this, _parentShape);
            InteractEventHandler = null;
            PregnantEventHandler = null;
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
           
            _otherShape = null;
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
            
            _otherShape = null;
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
            _pivot.StrokeColor = MediaColor.FromRgb(255, 255, 255);
            other.StrokeColor = MediaColor.FromRgb(255, 255, 255);
        }

        private void LightBorderAndPush(object source, BaseShape other)
        {
            other.Y += _pivot.Height+8.8;
        }

        private void LightBorderAndExpand(object source, BaseShape other)
        {
            if (other == null)
            {
                return;
            }
            _pivot.StrokeColor = MediaColor.FromRgb(255, 255, 255);
            other.StrokeColor = MediaColor.FromRgb(255, 255, 255);

           
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
            _otherShape.StrokeColor = Transparent;
        }
        /// <summary>
        /// what do our pivot interact with.
        /// </summary>
        /// <returns></returns>
        private BaseShape Watch()
        {
            // remove itself and on the same linked list.
            var interactSet = Parent.Where(i =>
            {
                if (i == _pivot)
                {
                    return false;
                }

                if (_pivot.Contain(i))
                {
                    return false;
                }

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
                if (_pivot.IsAtMiddle(i))
                {
                    InteractEventHandler = null;
                    WarningExpandEventHandler = null;
                    InteractEventHandler += MagnetMiddle;
                    WarningExpandEventHandler += LightBorderAndExpand;

                    return true;
                }

                // drag to bottom of other
                if (_pivot.IsAtBottom(i))
                {
                    InteractEventHandler = null;
                    WarningEventHandler = null;
                    InteractEventHandler += MagnetBot;
                    WarningEventHandler += LightBorder;

                    return true;
                }
                // drag to above of other
                if (_pivot.IsAtAbove(i))
                {
                    InteractEventHandler = null;
                    WarningEventHandler = null;
                    InteractEventHandler += MagnetTop;
                    WarningEventHandler += LightBorder;

                    return true;
                }
                return false;
            });

            return temp;
        }
    }
}
