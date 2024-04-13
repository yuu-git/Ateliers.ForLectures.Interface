using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ateliers.ForLectures.Interface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // サンプルを実行したいプログラムに合わせて、コメントアウトを解除してください。

            /* 01-01 疎結合を目的としたインターフェース使用の例 */
            // new LooseCoupling.LooseCouplingSample().Execute();

            /* 02-01 クラス機能の保証を目的としたインターフェース使用の例 */
            // new FunctionGuarantee.FunctionGuarantee().Execute();

            /* 03-01 インターフェースを使用した安全なアクセスの例 */
            // new SafeAccess.SafeAccess().Execute();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
