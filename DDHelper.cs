
namespace DDSoft
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Runtime.InteropServices;


    public enum KeyModifiers
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8
    }

    //public enum DDKeyCode
    //{
    //    DD_ESC = 100,
    //    DD_F1 = 101,
    //    DD_F2 = 102,
    //    DD_F3 = 103,
    //    DD_F4 = 104,
    //    DD_F5 = 105,
    //    DD_F6 = 106,
    //    DD_F7 = 107,
    //    DD_F8 = 108,
    //    DD_F9 = 109,
    //    DD_F10 = 110,
    //    DD_F11 = 111,
    //    DD_F12 = 112,
    //    DD_Tilde = 200,//波浪号
    //    DD_Num1 = 201,
    //    DD_Num2 = 202,
    //    DD_Num3 = 203,
    //    DD_Num4 = 204,
    //    DD_Num5 = 205,
    //    DD_Num6 = 206,
    //    DD_Num7 = 207,
    //    DD_Num8 = 208,
    //    DD_Num9 = 209,
    //    DD_Num0 = 210,
    //    DD_Sub = 211,
    //    DD_Equal = 212,
    //    DD_BackSlash = 213,//反斜杠\
    //    DD_BackSpace = 214,//退格键
    //    DD_TAB = 300,
    //    DD_Q = 301,
    //    DD_W = 302,
    //    DD_E = 303,
    //    DD_R = 304,
    //    DD_T = 305,
    //    DD_Y = 306,
    //    DD_U = 307,
    //    DD_I = 308,
    //    DD_O = 309,
    //    DD_P = 310,
    //    DD_LeftSquareBracket = 311,//左中括号[
    //    DD_RightSquareBracket = 312,//右中括号]
    //    DD_Enter = 313,
    //    DD_CapLock = 400,
    //    DD_A = 401,
    //    DD_S = 402,
    //    DD_D = 403,
    //    DD_F = 404,
    //    DD_G = 405,
    //    DD_H = 406,
    //    DD_J = 407,
    //    DD_K = 408,
    //    DD_L = 409,
    //    DD_Semicolon = 410,//分号;
    //    DD_SingleQuotationMark = 411,//单引号'
    //    DD_LeftShift = 500,
    //    DD_Z = 501,
    //    DD_X = 502,
    //    DD_C = 503,
    //    DD_V = 504,
    //    DD_B = 505,
    //    DD_N = 506,
    //    DD_M = 507,
    //    DD_Comma = 508,//逗号,
    //    DD_Dot = 509,//点.
    //    DD_Slash = 510,// 斜杠/
    //    DD_RightShift = 511,
    //    DD_LeftCtrl = 600,
    //    DD_LeftWin = 601,
    //    DD_LeftAlt = 602,
    //    DD_Space = 603,
    //    DD_RightAlt = 604,
    //    DD_RightWin = 605,
    //    DD_RightCtrl = 607,
    //    DD_PrintSceen = 700,
    //    DD_Scroll = 701,
    //    DD_Pause = 702,
    //    DD_Insert = 703,
    //    DD_Home = 704,
    //    DD_PageUp = 705,
    //    DD_Del = 706,
    //    DD_End = 707,
    //    DD_PageDown = 708,
    //    DD_Up = 709,
    //    DD_Left = 710,
    //    DD_Down = 711,
    //    DD_Right = 712,
    //    DD_NumPad0 = 800,
    //    DD_NumPad1 = 801,
    //    DD_NumPad2 = 802,
    //    DD_NumPad3 = 803,
    //    DD_NumPad4 = 804,
    //    DD_NumPad5 = 805,
    //    DD_NumPad6 = 806,
    //    DD_NumPad7 = 807,
    //    DD_NumPad8 = 808,
    //    DD_NumPad9 = 809,
    //    DD_NumLock = 810,
    //    DD_NumPadDivision = 811, //小键盘除/
    //    DD_NumPadMultiplication = 812,//小键盘乘*
    //    DD_NumPadSubtraction = 813,//小键盘减-
    //    DD_NumPadAdd = 814,//小键盘加+
    //    DD_NumPadEnter = 815,//小键盘Enter
    //}
    public class DDHelper
    {

        public DDHelper(string dllfile = null)
        {
            if (dllfile == null)
            {
                dllfile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dd2023.x64.dll");
            }
            Load(dllfile);

            this.MouseBtn(0);
        }
        [DllImport("Kernel32")]
        private static extern System.IntPtr LoadLibrary(string dllfile);

        [DllImport("Kernel32")]
        private static extern System.IntPtr GetProcAddress(System.IntPtr hModule, string lpProcName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);


        public delegate int pDD_MouseBtn(int btn);
        public delegate int pDD_MoustWheel(int whl);
        public delegate int pDD_KeyBoard(int ddcode, int flag);
        public delegate int pDD_MoustMove(int x, int y);
        public delegate int pDD_MouseMoveR(int dx, int dy);
        public delegate int pDD_InputStr(string str);
        public delegate int pDD_VK2Code(int vkcode);

        public pDD_MouseBtn MouseBtn;         //Mouse button 
        public pDD_MoustWheel MoustWheel;         //Mouse wheel
        public pDD_MoustMove MoustMove;      //Mouse move abs. 
        public pDD_MouseMoveR MouseMoveR;  //Mouse move rel. 
        public pDD_KeyBoard KeyBoard;         //Keyboard 
        public pDD_InputStr InputStr;            //Input visible char
        public pDD_VK2Code VK2Code;      //VK to ddcode
        private System.IntPtr m_hinst;

     


        ~DDHelper()
        {
            if (!m_hinst.Equals(IntPtr.Zero))
            {
                bool b = FreeLibrary(m_hinst);
            }
        }


        public int Load(string dllfile)
        {
            m_hinst = LoadLibrary(dllfile);
            if (m_hinst.Equals(IntPtr.Zero))
            {
                return -2;
            }
            else
            {
                return GetDDfunAddress(m_hinst);
            }
        }

        private int GetDDfunAddress(IntPtr hinst)
        {
            IntPtr ptr;

            ptr = GetProcAddress(hinst, "DD_btn");
            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            MouseBtn = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_MouseBtn)) as pDD_MouseBtn;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_whl");
            MoustWheel = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_MoustWheel)) as pDD_MoustWheel;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_mov");
            MoustMove = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_MoustMove)) as pDD_MoustMove;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_key");
            KeyBoard = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_KeyBoard)) as pDD_KeyBoard;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_movR");
            MouseMoveR = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_MouseMoveR)) as pDD_MouseMoveR;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_str");
            InputStr = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_InputStr)) as pDD_InputStr;

            if (ptr.Equals(IntPtr.Zero)) { return -1; }
            ptr = GetProcAddress(hinst, "DD_todc");
            VK2Code = Marshal.GetDelegateForFunctionPointer(ptr, typeof(pDD_VK2Code)) as pDD_VK2Code;

            return 1;
        }
    }

}


