using System.Diagnostics.Tracing;
using System.Text;
using System.Xml;
string[,] sBox = new string[,]
{
    {"63", "7c", "77", "7b", "f2", "6b", "6f", "c5", "30", "01", "67", "2b", "fe", "d7", "ab", "76"},
    {"ca", "82", "c9", "7d", "fa", "59", "47", "f0", "ad", "d4", "a2", "af", "9c", "a4", "72", "c0"},
    {"b7", "fd", "93", "26", "36", "3f", "f7", "cc", "34", "a5", "e5", "f1", "71", "d8", "31", "15"},
    {"04", "c7", "23", "c3", "18", "96", "05", "9a", "07", "12", "80", "e2", "eb", "27", "b2", "75"},
    {"09", "83", "2c", "1a", "1b", "6e", "5a", "a0", "52", "3b", "d6", "b3", "29", "e3", "2f", "84"},
    {"53", "d1", "00", "ed", "20", "fc", "b1", "5b", "6a", "cb", "be", "39", "4a", "4c", "58", "cf"},
    {"d0", "ef", "aa", "fb", "43", "4d", "33", "85", "45", "f9", "02", "7f", "50", "3c", "9f", "a8"},
    {"51", "a3", "40", "8f", "92", "9d", "38", "f5", "bc", "b6", "da", "21", "10", "ff", "f3", "d2"},
    {"cd", "0c", "13", "ec", "5f", "97", "44", "17", "c4", "a7", "7e", "3d", "64", "5d", "19", "73"},
    {"60", "81", "4f", "dc", "22", "2a", "90", "88", "46", "ee", "b8", "14", "de", "5e", "0b", "db"},
    {"e0", "32", "3a", "0a", "49", "06", "24", "5c", "c2", "d3", "ac", "62", "91", "95", "e4", "79"},
    {"e7", "c8", "37", "6d", "8d", "d5", "4e", "a9", "6c", "56", "f4", "ea", "65", "7a", "ae", "08"},
    {"ba", "78", "25", "2e", "1c", "a6", "b4", "c6", "e8", "dd", "74", "1f", "3b", "bd", "8b", "8a" },
    {"70", "3e", "b5", "66", "48", "03", "f6", "0e", "61", "35", "57", "b8", "86","c1", "1d", "9e"},
    {"e1", "f8", "98", "11", "69", "d9", "8e", "94", "9b", "1e", "87", "e9", "ce","55", "28", "df"},
    {"8c", "a1", "89", "0d", "bf", "e6", "42", "68", "41", "99", "2d", "0f", "b0","54", "bb", "16"}
    };
Console.WriteLine("Введите слово");
string text;
text = Console.ReadLine();
int n = text.Length, l = 0, x = 0, y = 0;
if (n < 16)
    for (int i = 0; i < 16 - n; i++)
        text += " ";
string binary_text = strtobit8(text);
Console.WriteLine(" слово в битах");
Console.WriteLine(binary_text);
string[,] bin_matrix = new string[4, 4], key_bin_matrix = new string[4, 44];
loop:
Console.WriteLine("Введите ключ ключ на 16 сиволов");
string key = Console.ReadLine();
if (key.Length < 16)
    goto loop;
string key_binary = strtobit8(key);

bin_matrix = strtomatrix(binary_text);
key_bin_matrix = strtomatrix(key_binary);
Console.WriteLine("Матрица слова");
print(bin_matrix);
Console.WriteLine("Матрица ключа");
print(key_bin_matrix);
AddRoundKey(ref bin_matrix, key_bin_matrix);
string[,] temp_key_matrix = new string[4, 4];
key_bin_matrix = key_expansion(key_bin_matrix, sBox);
int beg, end, p = 0, t = 0;

for (int round = 2; round <= 10; round++)
{
    p = 0;
    t = 0;
    end = round * 4;
    beg = (round * 4) - 4;
    for (int i = 0; i < 4; i++)
    {
        p = 0;
        for (int j = beg; j < end; j++)
        {
            temp_key_matrix[t, p] = key_bin_matrix[i, j];
            p++;
        }
        t++;
    }
    SubBytes(ref bin_matrix, sBox);
    ShfitRows(ref bin_matrix);
    MixColums(ref bin_matrix);
    AddRoundKey(ref bin_matrix, temp_key_matrix);
    Console.WriteLine("round" + (round - 1).ToString());
    print(bin_matrix);

}
SubBytes(ref bin_matrix, sBox);
ShfitRows(ref bin_matrix);
end = 11 * 4;
beg = (11 * 4) - 4;
t = 0;
for (int i = 0; i < 4; i++)
{
    p = 0;
    for (int j = beg; j < end; j++)
    {
        temp_key_matrix[t, p] = key_bin_matrix[i, j];
        p++;
    }
    t++;
}
AddRoundKey(ref bin_matrix, temp_key_matrix);
Console.WriteLine("Final round");
print(bin_matrix);
string final_word = "";
for (int i = 0; i < 4; i++)
{
    for (int j = 0; j < 4; j++)
    {
        int decimalNumber = Convert.ToInt32(bin_matrix[j, i], 2); // переводим в десятичное число
        char asciiChar = Convert.ToChar(decimalNumber); // переводим в символ ASCII
        string result = asciiChar.ToString(); // конвертируем символ в строку        
        final_word += result;
    }
}
Console.WriteLine("Зашифрованное слово\n" + final_word);
static void SubBytes(ref string[,] bin_matrix, string[,] sBox)
{
    string hex1, hex2;
    int x = 0, y = 0;
    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            //Console.Write(bin_matrix[i, j] + "\t");
            hex1 = bintohex(bin_matrix[i, j].Substring(0, 4), out x);
            hex2 = bintohex(bin_matrix[i, j].Substring(4, 4), out y);
            //Console.Write(x+":"+hex1 +"-" +y+":"+ hex2 + "\t");
            bin_matrix[i, j] = hextobit8(sBox[x, y]);
            //   Console.Write("new hex:{0}\t", sBox[x, y]);
        }
        //Console.WriteLine();
    }
}
static void MixColums(ref string[,] bin_matrix)
{
    string[,] mBox = new string[,]
    {
       {"02","03","01","01"},
       {"01","02","03","01"},
       {"01","01","02","03"},
       {"03","01","01","01"}
    };
    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            string str1 = bin_matrix[j, i];
            string str2 = hextobit8(mBox[j, i]);
            bin_matrix[j, i] = "";
            for (int h = 0; h < 8; h++)
            {
                bin_matrix[j, i] += str1[h] == str2[h] ? "0" : "1";
            }
        }

    }
}
static void ShfitRows(ref string[,] bin_matrix)
{
    string temp = bin_matrix[1, 0];
    bin_matrix[1, 0] = bin_matrix[1, 1];
    bin_matrix[1, 1] = bin_matrix[1, 2];
    bin_matrix[1, 2] = bin_matrix[1, 3];
    bin_matrix[1, 3] = temp;

    temp = bin_matrix[2, 0];
    string temp1 = bin_matrix[2, 1];
    bin_matrix[2, 0] = bin_matrix[2, 2];
    bin_matrix[2, 1] = bin_matrix[2, 3];
    bin_matrix[2, 2] = temp;
    bin_matrix[2, 3] = temp1;

    temp = bin_matrix[3, 0];
    temp1 = bin_matrix[3, 1];
    string temp2 = bin_matrix[3, 2];
    bin_matrix[3, 0] = bin_matrix[3, 3];
    bin_matrix[3, 1] = temp;
    bin_matrix[3, 2] = temp1;
    bin_matrix[2, 3] = temp2;
}
static void AddRoundKey(ref string[,] key_bin_matrix, string[,] bin_matrix)
{
    for (int j = 0; j < 4; j++)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int s = 0; s < 8; s++)
                bin_matrix[i, j] += bin_matrix[i, j].Substring(s, 1) ==
                    key_bin_matrix[i, j].Substring(s, 1) ? "0" : "1";
        }
    }
}
static void print(string[,] bin_matrix)
{
    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            Console.Write(bin_matrix[i, j] + "\t");
        }
        Console.WriteLine();
    }
}
static string strtobit8(string word)
{
    string myBits = "";
    foreach (char myChar in word)
    {
        byte myByte = (byte)myChar;
        myBits += Convert.ToString(myByte, 2).PadLeft(8, '0');

    }
    return myBits;
}
static string bintohex(string binaryNumber, out int coord)
{
    int decimalNumber = Convert.ToInt32(binaryNumber, 2);
    coord = decimalNumber;
    string hexNumber = Convert.ToString(decimalNumber, 16).ToUpper();
    return hexNumber;
}
static string hextobit8(string hexString)
{
    string binaryString = Convert.ToString(Convert.ToInt32(hexString, 16), 2).PadLeft(8, '0');
    return binaryString;
}
static string[,] key_expansion(string[,] start_key, string[,] sBox)
{
    string[,] Rcon = new string[4, 10]
    {
        {"01","02","04","08","10","20","40","80","1b","36"},
        {"00","00","00","00","00","00","00","00","00","00"},
        {"00","00","00","00","00","00","00","00","00","00"},
        {"00","00","00","00","00","00","00","00","00","00"},
    };
    string[,] key = new string[4, 44];
    for (int i = 0; i < 4; i++)
    {
        for (int j = 0; j < 4; j++)
        {
            key[i, j] = start_key[i, j];
        }
    }
    int l = 4, x = 0, y = 0;


    for (int j = 4; j < 44; j++)
    {
        string hex1, hex2;
        string[] temp_matrix = new string[4];
        string[] temp_matrix1 = new string[4];
        if (j == l)
        {

            for (int i = 0; i < 4; i++)
            {
                temp_matrix[i] = key[i, j - 1];
                temp_matrix1[i] = key[i, j - 4];
            }
            string temp = temp_matrix[0];
            string[] Rconcolumn = new string[4];
            temp_matrix[0] = temp_matrix[3];
            temp_matrix[3] = temp;
            for (int i = 0; i < 4; i++)
            {
                hex1 = bintohex(temp_matrix[i].Substring(0, 4), out x);
                hex2 = bintohex(temp_matrix[i].Substring(4, 4), out y);
                temp_matrix[i] = hextobit8(sBox[x, y]);
                Rconcolumn[i] = hextobit8(Rcon[i, (l / 4) - 1]);
            }
            for (int i = 0; i < 4; i++)
            {
                for (int s = 0; s < 8; s++)
                {
                    key[i, j] += temp_matrix[i].Substring(s, 1) == temp_matrix1[i].Substring(s, 1) &&
                        temp_matrix[i].Substring(s, 1) == Rconcolumn[i].Substring(s, 1)
                        && temp_matrix1[i].Substring(s, 1) == Rconcolumn[i].Substring(s, 1) ? "0" : "1";
                }
            }
            l += 4;
        }
        for (int i = 0; i < 4; i++)
        {
            temp_matrix[i] = key[i, j - 1];
            temp_matrix1[i] = key[i, j - 4];
        }
        for (int i = 0; i < 4; i++)
        {
            for (int s = 0; s < 8; s++)
            {
                key[i, j] += temp_matrix[i].Substring(s, 1) == temp_matrix1[i].Substring(s, 1) ? "0" : "1";
            }
        }
    }

    return key;
}
static string[,] strtomatrix(string word_binary)
{
    string[,] bin_matrix = new string[4, 4];
    int l = 0;
    for (int j = 0; j < 4; j++)
    {
        for (int i = 0; i < 4; i++)
        {
            try
            {
                bin_matrix[i, j] = word_binary.Substring(l, 8);
                l += 8;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
    return bin_matrix;
}
