using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TcpClientExample;

namespace TcpServerExample
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // 1. 서버 폼은 객체를 생성하고 'Show()' 메서드로 화면에 띄웁니다.
            ServerForm serverForm = new ServerForm();
            serverForm.Show();

            // 2. 클라이언트 폼은 애플리케이션의 메인 루프인 'Application.Run()'으로 실행합니다.
            Application.Run(new ClientForm());
        }
    }
}
