using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ateliers.ForLectures.Interface.LooseCoupling
{
    // このコードに必要なクラスやインターフェースは、本ファイルである 01-01.LooseCoupling.cs に全て記述しています。
    // このコードを理解するために、他のファイルを確認する必要はありません。

    /// <summary>
    /// LooseCoupling - 疎結合を目的としたインターフェース使用の例
    /// </summary>
    /// <example>
    /// 以下のように実行してください。 <br/>
    /// new LooseCouplingSample().Execute();
    /// </example>
    public class LooseCouplingSample
    {
        #region このコードについての説明
        /* -----------------------------------------------------------------------------------------------------------------------------------------------------------
            このコードは、メソッドが引数で受け入れる IEnumerable<T> と List<T> の違いを示すための例となっています。

            ①
            このコードでは、NewShoppingCartとOldShoppingCartの2つのショッピングカートが作成されています。
            NewShoppingCartはIEnumerable<IProduct>を受け入れるAddItemsメソッドを持ち、OldShoppingCartはList<IProduct>を受け入れるAddItemsメソッドを持っています。

            ②
            次に、リスト、配列、ソートされたリストの3つの異なるコレクションが作成され、それぞれに異なる商品が含まれています。

            ③
            【インターフェースとして受け入れる NewShoppingCart】
            NewShoppingCartのAddItemsメソッドは、これらのすべてのコレクションを受け入れることができます。
            これは、IEnumerable<T>がリスト、配列、ソートされたリストなど、様々なコレクション型に対する一般的なインターフェースであるためです。

            ④
            【インターフェースとして受け入れない OldShoppingCart】
            一方、OldShoppingCartのAddItemsメソッドは、リストのみを受け入れ、配列やソートされたリストを受け入れることはできません。
            これは、List<T>がIEnumerable<T>よりも具体的なコレクション型であるためです。

            まとめ：
            このコードは、IEnumerable<T>とList<T>の間の柔軟性の違いを明確に示しています。
            IEnumerable<T>を使用することで、より多くの種類のコレクションを受け入れることができ、コードの再利用性が向上します。
         ----------------------------------------------------------------------------------------------------------------------------------------------------------- */
        #endregion

        /// <summary>
        /// サンプルコードを実行し、結果を表示します。
        /// </summary>
        /// <remarks>
        /// 各 ShoppingCart クラスは、この Execute メソッドのすぐ下に定義を記述してありますので、併せて確認してみてください。
        /// </remarks>
        public void Execute()
        {
            // ① インターフェースリストを受け入れるカートと、受け入れないカートを作る
            var newCart = new NewShoppingCart();
            var oldCart = new OldShoppingCart();

            // ② List で商品リストを作成
            var listItems = new List<IProduct>
            {
                new Product("Apple", 100),
                new Product("Banana", 50),
                new Product("Orange", 80)
            };

            // ② array で商品リストを作成
            var arrayItems = new IProduct[]
            {
                new Product("Grapes", 120),
                new Product("Watermelon", 200),
                new Product("Pineapple", 150)
            };

            // ② SortedList で商品リストを作成
            var sortedListItems = new SortedList<int, IProduct>
            {
                { 1, new Product("Tomato", 90) },
                { 2, new Product("Potato", 70) },
                { 3, new Product("Carrot", 60) }
            };

            // 以下で追加メソッドを呼び出す
            // ③ newCart は、コレクションで広く受け入れられるので、全て追加できる
            newCart.AddItems(listItems);
            newCart.AddItems(arrayItems);
            newCart.AddItems(sortedListItems.Values);

            // ④ oldCart は、リストでしか受け取れないので、リスト以外は追加できない
            oldCart.AddItems(listItems);
            // oldCart.AddItems(arrayItems); // コンパイルエラー
            // oldCart.AddItems(sortedListItems.Values); // コンパイルエラー

            // 結果を表示
            Console.WriteLine("New Cart Items:");
            foreach (var item in newCart.items)
            {
                Console.WriteLine($"{item.Name} - {item.Price}");
            }

            // 結果を表示
            Console.WriteLine("Old Cart Items:");
            foreach (var item in oldCart.items)
            {
                Console.WriteLine($"{item.Name} - {item.Price}");
            }
        }
    }

    /// <summary>
    /// ショッピングカート
    /// </summary>
    public class NewShoppingCart : IShoppingCart<IProduct>
    {
        /// <summary> カート内の商品リスト </summary>
        private readonly List<IProduct> _Items = new List<IProduct>();

        /// <inheritdoc/>
        public IEnumerable<IProduct> items => _Items;

        /// <summary>
        /// ショッピングカートへのアイテム追加
        /// </summary>
        /// <param name="items"> 追加商品「コレクション」 </param>
        /// <remarks> このメソッドは、コレクションで広く受け入れられる例。 </remarks>
        public void AddItems(IEnumerable<IProduct> items)
        {
            _Items.AddRange(items);
        }
    }

    /// <summary>
    /// ショッピングカート
    /// </summary>
    public class OldShoppingCart : IShoppingCart<IProduct>
    {
        /// <summary> カート内の商品リスト </summary>
        private readonly List<IProduct> _Items = new List<IProduct>();

        /// <inheritdoc/>
        public IEnumerable<IProduct> items => _Items;

        /// <summary>
        /// ショッピングカートへのアイテム追加
        /// </summary>
        /// <param name="items"> 追加商品「リスト」 </param>
        /// <remarks> このメソッドは、リストでしか受け取れない例。 </remarks>
        public void AddItems(List<IProduct> items)
        {
            _Items.AddRange(items);
        }
    }

    #region --- その他のコードを確認したい方は、この region を開いてください ---

    /// <summary>
    /// 商品インターフェース
    /// </summary>
    public interface IProduct
    {
        /// <summary> 商品名 </summary>
        string Name { get; }
        /// <summary> 価格 </summary>
        decimal Price { get; }
    }

    /// <summary>
    /// ショッピングカートインターフェース
    /// </summary>
    public interface IShoppingCart<T> where T : class
    {
        /// <summary> カート内の商品リスト </summary>
        IEnumerable<T> items { get; }

        // ↓本当は実装したいが、説明の都合、ビルドエラーになるためコメントアウト
        // void AddItems(IEnumerable<T> items);
    }

    /// <summary>
    /// 商品クラス
    /// </summary>
    public class Product : IProduct
    {
        /// <inheritdoc/>
        public string Name { get; }
        /// <inheritdoc/>
        public decimal Price { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"> 商品名 </param>
        /// <param name="price"> 価格 </param>
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    #endregion
}
