using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            int[] keys = new int[] {72, 69, 89, 32, 89, 79, 85, 32, 68, 73, 68, 32, 73, 84, 32, 87, 82, 79, 78, 71,
                37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 37, 188, 39, 39, 39, 39,
                39, 39, 39, 39, 39, 39, 39, 39, 39, 39, 46, 46, 8, 8, 46, 46, 8};
            var a = CaKhia(keys);
            Console.WriteLine(a);
        }

        static string CaKhia(int [] keyCodes)
        {
            string res = "";
            int pointer = 0;
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (isBackSpace(keyCodes[i]))
                {
                    res = res.Substring(0, res.Length - 1);
                    if (pointer>0)
                    {
                        pointer--;
                    }
                    
                }else if (isDelete(keyCodes[i]))
                {
                    if (pointer<=res.Length)
                    {
                        string fist = res.Substring(0, pointer);
                        string second = res.Substring(pointer, res.Length - pointer);
                        string secondAgain = second.Substring(1, second.Length - 1);
                        res = fist + secondAgain;
                    }
                }
                else if (isEnd(keyCodes[i]))
                {
                    pointer = res.Length;
                }else if (isHome(keyCodes[i]))
                {
                    pointer = 0;
                }else if (isLeft(keyCodes[i]))
                {
                    if (pointer > 0)
                    {
                        pointer--;
                    }
                }
                else if (isRight(keyCodes[i]))
                {
                    if (pointer<res.Length)
                    {
                        pointer++;
                    }
                }

                else
                {
                    res = Write(res, ref pointer, keyCodes[i]);
                }
                
            }


            return res.ToLower();
        }

        static bool isEnd(int number)
        {
            return number == 35;
        }
        static bool isHome(int number)
        {
            return number == 36;
        }
        static bool isLeft(int number)
        {
            return number == 37;
        }
        static bool isRight(int number)
        {
            return number == 39;
        }
        // xoa phai
        static bool isDelete(int number)
        {
            return number == 46;
        }

        static bool isBackSpace(int number)
        {
            return number == 8;
        }

        static string Write(string res, ref int pointer, int keycode)
        {
            string result = "";
            //home
            if (pointer == 0)
            {
                result = convert(keycode) + res;
            }else if (pointer == res.Length) // end
            {
                result = res + convert(keycode);
                
            }else // left-right
            {
                string fist = res.Substring(0, pointer);
                string second = res.Substring(pointer, res.Length-pointer);
                result = fist + convert(keycode) + second;
            }
            pointer++;
            return result;
        }

        static string convert(int keycode)
        {
            if (keycode ==186)
            {
                return ";";
            }

            if (keycode == 188)
            {
                return ",";
            }

            if (keycode == 190)
            {
                return ".";
            }
            if (keycode == 222)
            {
                return "'";
            }

            return Convert.ToChar(keycode).ToString();
        }
    }
}
