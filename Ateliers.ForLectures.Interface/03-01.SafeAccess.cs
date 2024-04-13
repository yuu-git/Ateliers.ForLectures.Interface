using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ateliers.ForLectures.Interface.SafeAccess
{
    // このコードに必要なクラスやインターフェースは、本ファイルである 03-01.SafeAccess.cs に全て記述しています。
    // このコードを理解するために、他のファイルを確認する必要はありません。

    /// <summary>
    /// SafeAccess - インターフェースを使用した安全なアクセスの例
    /// </summary>
    /// <example>
    /// 以下のように実行してください。 <br/>
    /// new SafeAccess().Execute();
    /// </example>
    public class SafeAccess
    {
        #region このコードについての説明
        /* -----------------------------------------------------------------------------------------------------------------------------------------------------------
        このコードは、インターフェースを使用してクラスの内部状態を安全にアクセスする方法を示しています。

        ここでは、銀行口座を表す `BankAccount` クラスと `NgBankAccount` クラスを定義しています。
        これらのクラスは、アカウントID、合計金額、取引履歴を保持し、取引を追加し、取引履歴を保存する機能を提供します。

        `BankAccount` クラスは `IBankAccount` インターフェースを実装しています。
        このインターフェースを通じて、クラスの内部状態を安全に公開しています。
        具体的には、取引履歴は `IEnumerable<string>` 型のプロパティとして公開されています。
        これにより、取引履歴の参照は可能ですが、追加や削除はできません。

        また、`BankAccount` クラスのコンストラクタは private で、`CreateBankAccount` メソッドを通じてのみインスタンスを作成できます。
        これにより、`BankAccount` クラスの内部状態はさらに保護されます。

        一方、`NgBankAccount` クラスはインターフェースを実装していません。
        そのため、取引履歴は `List<string>` 型のプロパティとして公開されており、リストの内容を直接変更することが可能です。

        このコードは、インターフェースを使用することで、クラスの内部状態を安全に保護し、クラスの使用方法を制限することができることを示しています。
        ----------------------------------------------------------------------------------------------------------------------------------------------------------- */
        #endregion

        /// <summary>
        /// サンプルコードを実行し、結果を表示します。
        /// </summary>
        /// <remarks>
        /// 銀行口座の取引履歴を追加し、保存するサンプルコードです。<br/>
        /// このコードを実行すると、インターフェースを使用した銀行口座と、直接クラスを使用した銀行口座の違いを確認できます。
        /// </remarks>
        public void Execute()
        {
            // ① 銀行口座「BankAccount」を作成。１つは IBankAccount インターフェースを返し、もう１つは直接クラスを返す。
            // BankAccount クラスは、コンストラクタが private であるため、通常の方法ではインスタンスを作ることができず、必ず IBankAccount インターフェースで保護される。
            var goodBankAccount = BankAccount.CreateBankAccount("1234567890");
            //　var newBankAccount = new BankAccount("1234567890"); // NG　これは、Public コンストラクタが無いため。 コンパイルエラー
            var ngBankAccount = new NgBankAccount("0987654321");

            // ② インターフェースである goodBankAccount は、取引履歴を隠蔽しているため、参照は可能だが追加や削除はできない
            goodBankAccount.AddTransaction(1000); // OK 正常な手続きで取引を追加
            Console.WriteLine(goodBankAccount.TransactionHistory.First());
            // goodBankAccount.TransactionHistory.Add("不正な取引がありました。金額: 1000000"); // NG　コンパイルエラー
            // goodBankAccount.TransactionHistory.Clear();  // NG　コンパイルエラー
            goodBankAccount.Save(); // OK

            // ③ インターフェースでない ngBankAccount は、その内容を隠蔽していないため、リスト内容の変更が可能
            ngBankAccount.AddTransaction(1000); // OK 正常な手続きで取引を追加
            Console.WriteLine(goodBankAccount.TransactionHistory.First());
            ngBankAccount.TransactionHistory.Add("不正な取引がありました。金額: 1000000");　// OK　…ではないんだけど、リストで公開されてるので、できちゃう
            ngBankAccount.TransactionHistory.Clear();  // OK　これはひどい
            ngBankAccount.TransactionHistory.Add("新しい取引です。金額: -1000000"); // 本当に勘弁して
            Console.WriteLine(goodBankAccount.TransactionHistory.First());
            ngBankAccount.Save(); // やめてくれたまえ
        }
    }

    /// <summary>
    /// 銀行口座インターフェース
    /// </summary>
    /// <remarks>
    /// 取引履歴を保護するため、TransactionHistory プロパティは IEnumerable で公開しています。
    /// </remarks>
    public interface IBankAccount
    {
        /// <summary> アカウントID </summary>
        string AcountId { get; }

        /// <summary> 合計金額 </summary>
        decimal TotalAmount { get; }

        /// <summary> 取引履歴 </summary>
        IEnumerable<string> TransactionHistory { get; }

        /// <summary>
        /// 取引を追加します。
        /// </summary>
        /// <param name="amount"> 金額 </param>
        void AddTransaction(decimal amount);

        /// <summary>
        /// アカウントを保存します。
        /// </summary>
        void Save();
    }

    /// <summary>
    /// 銀行口座クラス
    /// </summary>
    /// <remarks>
    /// このクラスは IBankAccount インターフェースを実装しています。<br/>
    /// また、コンストラクタは private であり、外部から直接インスタンス化することはできず、ファクトリーメソッドから返されるのは IBankAccount インターフェースです。<br/>
    /// さらに sealed 修飾子を付けることで、このクラスを継承することを禁止し、内部値の安全性を高めて保護しています。
    /// </remarks>
    public sealed class BankAccount : IBankAccount
    {
        /// <inheritdoc/>
        public string AcountId { get; private set; }

        /// <inheritdoc/>
        public decimal TotalAmount { get; private set; }

        /// <summary> 取引履歴の実体 </summary>
        private readonly List<string> _transactionHistory = new List<string>();

        /// <inheritdoc/>
        public IEnumerable<string> TransactionHistory => _transactionHistory;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="accountId"> アカウントID </param>
        /// <remarks>
        /// このコンストラクタは private です。新規の口座は CreateBankAccount メソッドを使用して作成します。<br/>
        /// BankAccount クラスは new によって直接インスタンス化することはできません。
        /// </remarks>
        private BankAccount(string accountId)
        {
            // 本来はここでアカウントIDの妥当性チェックを行うが、例のため省略

            AcountId = accountId;
        }

        /// <summary>
        /// 銀行口座を作成します。
        /// </summary>
        /// <param name="accountId"> アカウントID </param>
        /// <returns> 銀行口座インタフェースを返します。</returns>
        /// <remarks>
        /// このメソッドは、BankAccount クラスのインスタンスを作成して返すファクトリメソッドです。<br/>
        /// このようにすることで、BankAccount クラスのインスタンスを外部から直接作成することを防ぎ、内部の状態を安全に保護します。
        /// </remarks>
        public static IBankAccount CreateBankAccount(string accountId)
        {
            return new BankAccount(accountId);
        }

        /// <inheritdoc/>
        public void AddTransaction(decimal amount)
        {
            // 本来はここで金額の妥当性や合計金額のチェックを行うが、例のため省略

            TotalAmount += amount;
            _transactionHistory.Add($"取引がありました。金額: {amount}");
        }

        /// <inheritdoc/>
        public void Save()
        {
            // ここでアカウントを保存する処理を実装
        }
    }

    /// <summary>
    /// 銀行口座クラス
    /// </summary>
    /// <remarks>
    /// インターフェースは実装していません。そのため、取引履歴リストが外部に公開されています。
    /// </remarks>
    public class NgBankAccount 
    {
        /// <summary> アカウントID </summary>
        public string AcountId { get; private set; }

        /// <summary> 合計金額 </summary>
        public decimal TotalAmount { get; private    set; }

        /// <summary> 取引履歴 </summary>
        public List<string> TransactionHistory { get; private set; } = new List<string>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="accountId"> アカウントID </param>
        public NgBankAccount(string accountId)
        {
            // 本来はここでアカウントIDの妥当性チェックを行うが、例のため省略

            AcountId = accountId;
        }

        /// <summary>
        /// 取引を追加します。
        /// </summary>
        /// <param name="amount"> 金額 </param>
        public void AddTransaction(decimal amount)
        {
            // 本来はここで金額の妥当性や合計金額のチェックを行うが、例のため省略

            TotalAmount += amount;
            TransactionHistory.Add($"取引がありました。金額: {amount}");
        }

        /// <summary>
        /// アカウントを保存します。
        /// </summary>
        public void Save()
        {
            // ここでアカウントを保存する処理を実装
        }
    }
}
