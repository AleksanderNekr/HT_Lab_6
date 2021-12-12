using System.Text.RegularExpressions;

namespace Lab_6;

internal static class Program
{
    private static void Main()
    {
        while (true)
        {
            Console.Write("Выберите, с чем работать:" +
                          "\n1. Рваный массив"        +
                          "\n2. Символьная строка"    +
                          "\n3. Завершить выполнение" +
                          " программы\nВаш выбор: ");

            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nВы выбрали работу с рваным массивом");
                    MenuArr();
                    break;
                case "2":
                    Console.WriteLine("\nВы выбрали работу со строками");
                    MenuString();
                    break;
                case "3":
                    Console.WriteLine("\nЗавершение программы...");
                    return;
                default:
                    Console.WriteLine(ErrUnknownChar);
                    break;
            }
        }
    }

    #region Рваный массив

    /// Меню для работы с рваным массивом
    private static void MenuArr()
    {
        char[][] jaggedArray = Array.Empty<char[]>();
        while (true)
        {
            Console.Write("\nВыберите, что сделать:"                +
                          "\n1. Создать рваный массив символов"     +
                          "\n2. Удалить из массива первую строку,"  +
                          " в которой есть не менее 3 гласных букв" +
                          "\n3. Вывести массив на экран"            +
                          "\n4. Вернуться в главное меню\nВаш выбор: ");
            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nВы выбрали создать рваный массив символов");
                    MenuArrCreate(ref jaggedArray);
                    break;
                case "2":
                    Console.WriteLine("\nВы выбрали удалить из массива первую строку," +
                                      " в которой есть не менее 3 гласных букв");
                    DeleteRowMore3Vowels(ref jaggedArray);
                    break;
                case "3":
                    Console.WriteLine("\nВы выбрали вывести массив на экран");
                    WriteArray(jaggedArray);
                    break;
                case "4":
                    Console.WriteLine(ReturnToMain);
                    return;
                default:
                    Console.WriteLine(ErrUnknownChar);
                    break;
            }
        }
    }

    /// Меню для заполнения рваного массива символами
    /// <param name="jaggedArrChars">Рваный массив</param>
    private static void MenuArrCreate(ref char[][] jaggedArrChars)
    {
        while (true)
        {
            Console.Write("\nВыберите, что сделать:"     +
                          "\n1. Ввести элементы вручную" +
                          "\n2. Сгенерировать элементы"  +
                          " с помощью датчика случайных" +
                          " чисел\n3. Вернуться в"       +
                          " предыдущее меню\nВаш выбор: ");

            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nВыбрано ввести элементы массива вручную");
                    ReadArray(out jaggedArrChars);
                    WriteArray(jaggedArrChars);
                    return;
                case "2":
                    Console.WriteLine("\nВыбрано сгенерировать элементы массива " +
                                      "с помощью датчика случайных чисел");
                    GenerateArray(out jaggedArrChars);
                    WriteArray(jaggedArrChars);
                    return;
                case "3":
                    Console.WriteLine("\nВы выбрали вернуться в предыдущее меню");
                    return;
                default:
                    Console.WriteLine(ErrUnknownChar);
                    break;
            }
        }
    }

    /// <summary>
    ///     Ручной ввод элементов рваного массива <see cref="T:System.Char" /> значений
    /// </summary>
    private static void ReadArray(out char[][] jaggedArrChars)
    {
        Console.Write(InputCountOf);
        ReadUint(out uint countOfRows);
        jaggedArrChars = new char[countOfRows][];

        if (countOfRows == 0) return;

        Console.WriteLine("\nСимволы вводите подряд, не разделяя ничем\n" +
                          "Для завершения ввода нажмите “Enter”");

        for (int i = 0; i < countOfRows; i++)
        {
            Console.Write($"Введите строку символов №{i + 1}: ");
            jaggedArrChars[i] = Console.ReadLine()!.ToCharArray();
        }

        Console.WriteLine(CreateArraySuccess);
    }

    /// <summary>
    ///     Генерация элементов <see cref="T:System.Char" /> значений
    ///     рваного массива с помощью датчика случайных чисел
    /// </summary>
    private static void GenerateArray(out char[][] jaggedArrInts)
    {
        Random generator = new();
        Console.Write(InputCountOf);
        ReadUint(out uint countOfRows);

        jaggedArrInts = new char[countOfRows][];
        for (int i = 0; i < countOfRows; i++)
        {
            int countOfCells = generator.Next(1, 11);
            jaggedArrInts[i] = new char[countOfCells];
            for (int j = 0; j < countOfCells; j++)
                jaggedArrInts[i][j] = Chars[generator.Next(Chars.Length)];
        }

        Console.WriteLine(CreateArraySuccess);
    }

    /// Удаление из рваного массива строки, содержащей не менее чем 3 гласных букв
    /// <param name="jaggedArray">Рваный массив</param>
    private static void DeleteRowMore3Vowels(ref char[][] jaggedArray)
    {
        if (jaggedArray.Length > 0)
        {
            for (uint indexOfRow = 0; indexOfRow < jaggedArray.Length; indexOfRow++)
                if (CountOfVowels(jaggedArray[indexOfRow]) >= 3)
                {
                    jaggedArray = jaggedArray.DeleteRow(indexOfRow);
                    Console.WriteLine($"Строка №{indexOfRow + 1} успешно удалена!");
                    WriteArray(jaggedArray);
                    return;
                }

            Console.WriteLine("\nНе найдено строки, в которой есть" +
                              " не менее 3 гласных букв!\nУдаление невозможно!");
        }
        else
        {
            Console.WriteLine("Массив пустой, удаление невозможно!");
        }
    }

    /// Удаление из рваного массива символов строки с заданным индексом
    /// <param name="jaggedArray">Рваный массив символов</param>
    /// <param name="index">Индекс удаляемой строки</param>
    private static char[][] DeleteRow(this char[][] jaggedArray, uint index)
    {
        // Поверхностное копирование строк с индексами, не равными заданному
        char[][] resultArray = new char[jaggedArray.Length - 1][];
        for (uint indexOfRow = 0; indexOfRow < index; indexOfRow++)
            resultArray[indexOfRow] = jaggedArray[indexOfRow];
        for (uint indexOfRow = index + 1; indexOfRow < jaggedArray.Length; indexOfRow++)
            resultArray[indexOfRow - 1] = jaggedArray[indexOfRow];

        return resultArray;
    }

    /// Подсчет количества гласных букв в массиве символов
    /// <param name="chars">Массив символов</param>
    private static int CountOfVowels(char[] chars)
    {
        return chars.Count(symbol => Vowels.Contains(symbol));
    }

    ///Вывод рваного массива в консоль
    private static void WriteArray<T>(T[][] jaggedArrInts)
    {
        if (jaggedArrInts.Length > 0)
            foreach (T[] row in jaggedArrInts)
            {
                foreach (T element in row)
                    Console.Write($"{element,2}");
                Console.WriteLine();
            }
        else
            Console.WriteLine("Массив не содержит элементов");
    }

    /// <summary>
    ///     Ввод числа типа <see cref="T:System.UInt32" />
    ///     <param name="uintNum">Число</param>
    /// </summary>
    private static void ReadUint(out uint uintNum)
    {
        bool isConvert;
        do
        {
            isConvert = uint.TryParse(Console.ReadLine(), out uintNum);
            if (!isConvert)
                Console.Write("\nОШИБКА! Введено нецелое число, или" +
                              " отрицательное, или не число!"        +
                              "\nВведите заново: ");
        } while (!isConvert);
    }

    #endregion

    #region Символьные строки

    // Меню для работы с рваным массивом
    private static void MenuString()
    {
        string str = "";
        while (true)
        {
            Console.Write("\nВыберите, что сделать:"             +
                          "\n1. Создать строку символов"         +
                          "\n2. Перевернуть в строке"            +
                          " каждое предложение, заканчивающееся" +
                          " символом ‘!’\n3. Вывести строку"     +
                          " на экран\n4. Вернуться в главное"    +
                          " меню\nВаш выбор: ");
            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nВы выбрали создать строку символов");
                    MenuCreateString(ref str);
                    break;
                case "2":
                    Console.WriteLine("\nВы выбрали перевернуть в строке"    +
                                      " каждое предложение, заканчивающееся" +
                                      " символом ‘!’");

                    string[] sentences = FindSentEndWithExclPoint(str);
                    ReverseSentences(ref str, sentences);
                    break;
                case "3":
                    Console.WriteLine("\nВы выбрали вывести строку на экран" +
                                      $"\n{(str.Length > 0 ? str : "Строка пуста")}");
                    break;
                case "4":
                    Console.WriteLine(ReturnToMain);
                    return;
                default:
                    Console.WriteLine(ErrUnknownChar);
                    break;
            }
        }
    }

    /// Меню для выбора метода создания строки
    private static void MenuCreateString(ref string str)
    {
        while (true)
        {
            Console.Write("\nВыберите, что сделать:"       +
                          "\n1. Ввести строку вручную"     +
                          "\n2. Выбрать строку из"         +
                          " строк, приготовленных заранее" +
                          "\n3. Вернуться в меню работы"   +
                          " со строками\nВаш выбор: ");

            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    Console.WriteLine("\nВыбрано ввести строку символов вручную" +
                                      "\nЗнаки препинания не повторяются!"       +
                                      "\nВведите строку:");
                    ReadCorrectString(out str);
                    return;
                case "2":
                    Console.WriteLine("\nВы выбрали использовать строки," +
                                      " приготовленные заранее");
                    MenuChooseString(out str);
                    Console.WriteLine(str);
                    return;
                case "3":
                    Console.WriteLine(ReturnToStringMenu);
                    return;
                default:
                    Console.WriteLine(ErrUnknownChar);
                    break;
            }
        }
    }

    /// Меню для выбора из заготовленных строк
    private static void MenuChooseString(out string str)
    {
        // Текст ошибочен пунктуационно, но это не важно
        string[] strings =
        {
            "Здравствуйте!",

            "Здравствуйте! Классы: StringBuilder, и String.",

            "Классы: StringBuilder и String. Они предоставляют" +
            " достаточную, функциональность: для работы!",

            "Они предоставляют"                                        +
            " достаточную, функциональность: для работы; со строками!" +
            " Однако, .NET предлагает еще один мощный инструмент.",

            "Однако, .NET предлагает; еще один: мощный инструмент" +
            " - регулярные выражения! Регулярные выражения"        +
            " представляют, эффективный. Также гибкий метод: по"   +
            " обработке больших, текстов!",

            "Однако .NET предлагает; еще один: мощный инструмент" +
            " - регулярные выражения! Позволяя в то же время"     +
            " существенно, уменьшить: объемы кода! По сравнению"  +
            " с использованием, стандартных строковых операций?"
        };

        while (true)
        {
            Console.Write("Выберите длину строки:"                   +
                          "\n1. Очень короткая (1 слово)"            +
                          "\n2. Короткая (5 слов)"                   +
                          "\n3. Средняя (10 слов)"                   +
                          "\n4. Длинная (15 слов)"                   +
                          "\n5. Очень длинная (20 слов)"             +
                          "\n6. Вернуться в меню работы со строками" +
                          "\nВаш выбор: ");

            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Вы выбрали строку с очень маленькой длиной");
                    str = strings[0];
                    return;
                case "2":
                    Console.WriteLine("Вы выбрали строку с маленькой длиной");
                    str = strings[1];
                    return;
                case "3":
                    Console.WriteLine("Вы выбрали строку со средней длиной");
                    str = strings[2];
                    return;
                case "4":
                    Console.WriteLine("Вы выбрали строку с большой длиной");
                    str = strings[3];
                    return;
                case "5":
                    Console.WriteLine("Вы выбрали строку с очень большой длиной");
                    str = strings[4];
                    return;
                case "6":
                    Console.WriteLine(ReturnToStringMenu);
                    str = strings[5];
                    return;
                default:
                    Console.WriteLine(ErrUnknownChar);
                    break;
            }
        }
    }

    /// Найти предложения, оканчивающиеся на воскл. знак
    /// <param name="inputStr">Исходная строка</param>
    /// <returns>Массив строк-предложений</returns>
    private static string[] FindSentEndWithExclPoint(string inputStr)
    {
        // \w – первый символ буква или цифра
        // [^\.\?]+ – последующие символы любые, кроме знаков окончания предложения, не считая "!"
        // ! – конечный смвол, конец нужного прредложения
        Regex regex = new(@"\w[^\.\?]+!");

        MatchCollection matches   = regex.Matches(inputStr);
        string[]        sentences = Array.Empty<string>();
        uint            index     = 0;

        if (inputStr.Length == 0)
        {
            Console.WriteLine("В строке нет символов, выполнение невозможно!");
            return sentences;
        }

        if (matches.Count > 0)
            foreach (Match match in matches)
            {
                Array.Resize(ref sentences, sentences.Length + 1);
                sentences[index++] = match.ToString();
            }
        else
            Console.WriteLine("\nСтрока не содержит корректных предложений," +
                              " оканчивающихся на ‘!’");

        return sentences;
    }

    /// Переворот предложений в строке
    /// <param name="inputStr">Строка</param>
    /// <param name="sentences">Предложения</param>
    private static void ReverseSentences(ref string inputStr, string[] sentences)
    {
        if (sentences.Length == 0) return;

        foreach (string sentence in sentences)
        {
            // Переворот предложения
            char[] newSentence = sentence.ToCharArray();
            Array.Reverse(newSentence);

            inputStr = inputStr.Replace(sentence, new string(newSentence));
        }

        Console.WriteLine("Предложения успешно перевернуты!");
        Console.WriteLine(inputStr);
    }

    /// Ввод строки с проверкой на повторяющиеся пунктуационные знаки
    /// <param name="input">Выходная строка</param>
    private static void ReadCorrectString(out string input)
    {
        bool isCorrect;

        // [^\w\s] – не буквенно-циферный символ
        // \s* – отделен от другого либо ничем, либо пробельным символом
        Regex regex = new(@"[^\w\s]\s*[^\w\s]");
        do
        {
            input     = Console.ReadLine()!;
            isCorrect = regex.Matches(input).Count == 0;
            Console.WriteLine(isCorrect
                                  ? "\nСтрока успешно введена!"
                                  : "\nОбнаружены повторяющиеся знаки!" +
                                    "\nВведите строку заново!");
        } while (!isCorrect);
    }

    #endregion

    #region Литерные сроки

    private const string ReturnToMain       = "\nВы выбрали вернуться в главное меню";
    private const string CreateArraySuccess = "\nМассив успешно создан!";

    /// Символы для заполнения массива
    private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                 "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"                    +
                                 "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789";

    /// Гласные буквы
    private const string Vowels = "eyuioaEYUIOAуеёыаоэяиюУЕЁЫАОЭЯИЮ";

    private const string ErrUnknownChar = "\nВы ввели неизвестный символ! Введите заново!";

    private const string InputCountOf = "\nВведите целое неотрицательное число" +
                                        " – количество строк рваного массива: ";

    private const string ReturnToStringMenu = "Вы выбрали вернуться в меню работы со строками";

    #endregion
}