using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ateliers.ForLectures.Interface.FunctionGuarantee
{
    // このコードに必要なクラスやインターフェースは、本ファイルである 02-01.FunctionGuarantee.cs に全て記述しています。
    // このコードを理解するために、他のファイルを確認する必要はありません。

    /// <summary>
    /// FunctionGuarantee - クラス機能の保証を目的としたインターフェース使用の例
    /// </summary>
    /// <example>
    /// 以下のように実行してください。 <br/>
    /// new FunctionGuarantee().Execute();
    /// </example>
    public class FunctionGuarantee
    {
        #region このコードについての説明
        /* -----------------------------------------------------------------------------------------------------------------------------------------------------------
        このコードは、インターフェースの使用方法とその利点を示すためのサンプルです。

        1. `Dog`、`Cat`、`Bird`という異なるクラスがありますが、それぞれが`IAnimal`というインターフェースを実装しています。
            これにより、これらのクラスは`Name`プロパティと`Speak`メソッドを持つことが保証されます。

        2. メインとなる`Execute`メソッドでは、`Dog`と`Bird`のインスタンスを`IAnimal`インターフェースとして作成し、`Cat`のインスタンスだけは直接クラスを使用して作成しています。
            これにより、`Dog`と`Bird`は`IAnimal`インターフェースのメソッドとプロパティのみにアクセスでき、`Cat`は`IAnimal`のメソッドとプロパティに加えて、`Cat`クラス固有のメソッドとプロパティにもアクセスできます。

        3. 同じクラス内の `ExecuteSpeek`メソッドと`GetAnimalsName`メソッドは、異なるクラスのオブジェクトでも、共通のインターフェースを実装していれば同じ方法で扱うことができるという、インターフェースの強力な利点を示しています。

        特に大事な項目は ② と ⑥ です。次点で ⑤ になります。
        これらの項目を理解することで、インターフェースの利点を理解することができます。
         ----------------------------------------------------------------------------------------------------------------------------------------------------------- */
        #endregion

        /// <summary>
        /// サンプルコードを実行し、結果を表示します。
        /// </summary>
        /// <remarks>
        /// インターフェース及び固有のクラスは、このファイルに定義を記述してありますので、併せて確認してみてください。
        /// </remarks>
        public void Execute()
        {
            // ① インターフェースとして、Dog, Bird のインスタンスを作成し、Cat だけはクラスインスタンスを作成
            IAnimal dog = new Dog("Pochi", "Golden Retriever");
            Cat cat = new Cat("Tama", 3);
            IAnimal bird = new Bird("Piko", "Blue");

            // ② dog と bird は IAnimal インターフェース、cat は Cat クラスであるが、全て IAnimal インターフェースを実装しているため、同じリストに格納することができる
            var animals = new List<IAnimal> { dog, cat, bird };

            // ③ ただし、dog と bird はインターフェースであるため、固有のプロパティやメソッドにはアクセスできない
            /* 以下のコードはビルドエラーとなる 
            console.writeline($"dog: Breed = {cat.Breed}");
            dog.Run();
            console.writeline($"bird: Color = {bird.Color}");
            bird.Fly();
            */

            // ④ cat だけは Cat クラスであるため、固有のプロパティやメソッドにアクセスできる
            Console.WriteLine($"④ cat: Age = {cat.Age}");
            cat.Sleep();

            Console.WriteLine("--------------------------------------------");

            // ⑤ インターフェースで「動物が鳴く」という機能は保証されているため、異なるクラスを束ねたリストであっても、ループ処理でインターフェースのSpeakメソッドを実行できる
            // この時、これらの出力は、インターフェースからアクセスすることのできないプロパティも、インターフェースを通じて出力できる
            // 具体的には Dog は Breed、Cat は Age、Bird は Color が出力される
            Console.WriteLine("⑤ Speak:");
            animals.ForEach(animal => animal.Speak());

            Console.WriteLine("--------------------------------------------");

            // ⑥ 効果的な使い方：異なるクラスでありながら、インタフェースとして共通の機能を持つため、引数で引き渡した先で保証されたインターフェースの機能を利用できる
            // これが一番大事 → (クラスの場合、引数の指定が異なるため、Dog Cat Bird をまとめて引き渡すことができない。これについての詳しい解説は LooseCoupling.cs を参照)
            ExecuteSpeek(animals);
            Console.WriteLine("--------------------------------------------");

            var names = GetAnimalsName(animals);
            Console.WriteLine("⑥ GetAnimalsName Result:");
            foreach (var name in names)
            {
                Console.WriteLine($"Animal Name: {name}");
            }

            Console.WriteLine("--------------------------------------------");

            // ⑦ 余談：インターフェースとして定義された dog と bird も、クラスとして固有のプロパティやメソッドは持っている。以下は、キャストしてメソッドを利用する例：
            Console.WriteLine("⑦ Dog.Run:");
            ((Dog)dog).Run();
            Console.WriteLine("⑦ Bird.Fly:");
            ((Bird)bird).Fly();
        }

        /// <summary>
        /// 動物達の鳴き声を出力する
        /// </summary>
        /// <param name="animals"> 動物コレクション </param>
        /// <remarks> 
        /// このメソッドは、異なるクラスのオブジェクトでも、共通のインターフェースを実装していれば、同じ方法で扱うことができるという、インターフェースの強力な利点を示しています。
        /// </remarks>
        private void ExecuteSpeek(IEnumerable<IAnimal> animals)
        {
            Console.WriteLine("⑥ PrivateMethod.ExecuteSpeek:");
            foreach (var animal in animals)
            {
                animal.Speak();
            }
        }

        /// <summary>
        /// 動物達の名前を取得する
        /// </summary>
        /// <param name="animals"> 動物コレクション </param>
        /// <remarks>
        /// このメソッドは、異なるクラスのオブジェクトでも、共通のインターフェースを実装していれば、同じ方法で扱うことができるという、インターフェースの強力な利点を示しています。
        /// </remarks>
        private IEnumerable<string> GetAnimalsName(IEnumerable<IAnimal> animals)
        {
            return animals.Select(animal => animal.Name).ToList();
        }
    }

    /// <summary>
    /// 動物インターフェース
    /// </summary>
    public interface IAnimal
    {
        /// <summary> 動物の名前 </summary>
        string Name { get; }
        /// <summary> 動物の鳴き声を出力 </summary>
        void Speak();
    }

    #region --- 固有の動物クラスの実装を確認したい方は、この region を開いてください ---

    /// <summary>
    ///　犬クラス
    /// </summary>
    public class Dog : IAnimal
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <summary>
        /// 犬種
        /// </summary>
        /// <remarks> Dog は固有の犬種を示すプロパティを持つ </remarks>
        public string Breed { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"> 犬の名前 </param>
        /// <param name="breed"> 犬種 </param>
        public Dog(string name, string breed)
        {
            Name = name;
            Breed = breed;
        }

        /// <inheritdoc/>
        public void Speak()
        {
            // Dog は 泣き声に加えて、犬種も表示できる
            Console.WriteLine($"{Name} ({Breed}) says: Woof!");
        }

        /// <summary>
        /// 犬固有の出力メソッド
        /// </summary>
        public void Run()
        {
            Console.WriteLine($"{Name} is running.");
        }
    }

    /// <summary>
    /// 猫クラス
    /// </summary>
    public class Cat : IAnimal
    {
        /// <inheritdoc/>
        public string Name { get; }
            
        /// <summary>
        /// 年齢 
        /// </summary>
        /// <remarks> Cat は固有の年齢を示すプロパティを持つ </remarks>
        public int Age { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"> 猫の名前 </param>
        /// <param name="age"> 年齢 </param>
        public Cat(string name, int age)
        {
            Name = name;
            Age = age;
        }

        /// <inheritdoc/>
        public void Speak()
        {
            // Cat は 泣き声に加えて、年齢も表示できる
            Console.WriteLine($"{Name} ({Age} years old) says: Meow!");
        }

        /// <summary>
        /// 猫固有の出力メソッド
        /// </summary>
        public void Sleep()
        {
            Console.WriteLine($"{Name} is sleeping.");
        }
    }

    /// <summary>
    /// 鳥クラス
    /// </summary>
    public class Bird : IAnimal
    {
        /// <inheritdoc/>
        public string Name { get; }

        /// <summary>
        /// 色
        /// </summary>
        /// <remarks> Bird は固有の色を示すプロパティを持つ </remarks>
        public string Color { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"> 鳥の名前 </param>
        /// <param name="color"> 色 </param>
        public Bird(string name, string color)
        {
            Name = name;
            Color = color;
        }

        /// <inheritdoc/>
        public void Speak()
        {
            // Bird は 泣き声に加えて、色も表示できる
            Console.WriteLine($"{Name} ({Color} bird) says: Tweet!");
        }

        /// <summary>
        /// 鳥固有の出力メソッド
        /// </summary>
        public void Fly()
        {
            Console.WriteLine($"{Name} flies away.");
        }
    }

    #endregion
}
