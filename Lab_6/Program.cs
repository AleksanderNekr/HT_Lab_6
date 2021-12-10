namespace Lab_6;

internal static class Program
{
    private static void Main()
    {
        while (true)
        {
            Console.Write(MainStart);

            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    Console.WriteLine(MainChoiceArr);
                    MenuArr();
                    break;
                case "2":
                    Console.WriteLine(MainChoiceStr);
//                    MenuString();
                    break;
                case "3":
                    Console.WriteLine(MainExit);
                    return;
                default:
                    Console.WriteLine(ErrUnknownChar);
                    break;
            }
        }
    }

    #region Литерные сроки сообщений

    private const string ReturnToMain         = "\nВы выбрали вернуться в главное меню";
    private const string ReturnToPreviousMenu = "\nВы выбрали вернуться в предыдущее меню";
    private const string CreateArraySuccess   = "\nМассив успешно создан!";

    private const string Chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                 "абвгдеёжзийклмнопрстуфхцчшщъыьэюя"                    +
                                 "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789";

    private const string Vowels     = "eyuioaEYUIOAуеёыаоэяиюУЕЁЫАОЭЯИЮ";
    private const string ArrayEmpty = "Массив не содержит элементов";

    #region Функция Main

    private const string MainStart = @"Выберите, с чем работать:
1. Рваный массив
2. Символьная строка
3. Завершить выполнение программы
Ваш выбор: ";

    private const string MainChoiceArr  = "\nВы выбрали работу с рваным массивом";
    private const string MainChoiceStr  = "\nВы выбрали работу со строками";
    private const string MainExit       = "\nЗавершение программы...";
    private const string ErrUnknownChar = "\nВы ввели неизвестный символ! Введите заново!";

    #endregion

    #region Функция MenuArr

    private const string MenuArrStart = "\n" + @"Выберите, что сделать:
1. Создать рваный массив символов
2. Удалить из массива первую строку, в которой есть не менее 3 гласных букв
3. Вывести массив на экран
4. Вернуться в главное меню
Ваш выбор: ";

    private const string MenuArrChoiceCreateArr = "\nВы выбрали создать рваный массив символов";

    private const string MenuArrChoiceDeleteRow = "\nВы выбрали удалить из массива первую строку, " +
                                                  "в которой есть не менее 3 гласных букв";

    private const string MenuArrChoiceWrite = "\nВы выбрали вывести массив на экран";

    #endregion

    #region Функция MenuArrCreate

    private const string MenuArrCreateStart = "\n" + @"Выберите, что сделать:
1. Ввести элементы вручную
2. Сгенерировать элементы с помощью датчика случайных чисел
3. Вернуться в предыдущее меню
Ваш выбор: ";

    private const string MenuArrCreateRead = "\nВыбрано ввести элементы массива вручную";

    private const string MenuArrCreateGenerate = "\nВыбрано сгенерировать элементы массива " +
                                                 "с помощью датчика случайных чисел";

    #endregion

    #region Функция ReadArray и GenerateArray

    private const string InputCountOf = "\nВведите целое неотрицательное число" +
                                        " – количество строк рваного массива: ";

    private const string ReadArrayWarn = "\nСимволы вводите подряд, не разделяя ничем\n" +
                                         "Для завершения ввода нажмите “Enter”";

    private const string ReadArrayInput = "Введите строку символов №";

    #endregion

    private const string DeleteFailFind = "\nНе найдено строки, в которой есть" +
                                          " не менее 3 гласных букв!\nУдаление невозможно!";

    private const string DeleteArrEmpty = "Массив пустой, удаление невозможно!";
    private const string DeleteSuccess  = "Строка успешно удалена!\nНомер строки: ";

    private const string ErrConvert = "\nОШИБКА! Введено нецелое число, или отрицательное, " +
                                      "или не число!\nВведите заново: ";

    #endregion

    #region Рваный массив

    /// Меню для работы с рваным массивом
    private static void MenuArr()
    {
        char[][] jaggedArray = Array.Empty<char[]>();
        while (true)
        {
            Console.Write(MenuArrStart);
            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    Console.WriteLine(MenuArrChoiceCreateArr);
                    MenuArrCreate(ref jaggedArray);
                    break;
                case "2":
                    Console.WriteLine(MenuArrChoiceDeleteRow);
                    DeleteRowMore3Vowels(ref jaggedArray);
                    break;
                case "3":
                    Console.WriteLine(MenuArrChoiceWrite);
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
            Console.Write(MenuArrCreateStart);

            string choice = Console.ReadLine()!;

            switch (choice)
            {
                case "1":
                    Console.WriteLine(MenuArrCreateRead);
                    ReadArray(out jaggedArrChars);
                    WriteArray(jaggedArrChars);
                    return;
                case "2":
                    Console.WriteLine(MenuArrCreateGenerate);
                    GenerateArray(out jaggedArrChars);
                    WriteArray(jaggedArrChars);
                    return;
                case "3":
                    Console.WriteLine(ReturnToPreviousMenu);
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
        Console.WriteLine(ReadArrayWarn);

        for (int i = 0; i < countOfRows; i++)
        {
            Console.Write(ReadArrayInput + (i + 1) + ": ");
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
                    Console.WriteLine(DeleteSuccess + (indexOfRow + 1));
                    return;
                }

            Console.WriteLine(DeleteFailFind);
        }
        else
        {
            Console.WriteLine(DeleteArrEmpty);
        }
    }

    /// Удаление из рваного массива символов строки с заданным индексом
    /// <param name="jaggedArray">Рваный массив символов</param>
    /// <param name="index">Индекс удаляемой строки</param>
    private static char[][] DeleteRow(this char[][] jaggedArray, uint index)
    {
        // Копирование строк с индексами, не равными заданному
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
                    Console.Write($"{element,5}");
                Console.WriteLine();
            }
        else
            Console.WriteLine(ArrayEmpty);
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
                Console.Write(ErrConvert);
        } while (!isConvert);
    }

    #endregion
}