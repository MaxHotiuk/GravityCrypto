using System;
using System.Text;
using System.Collections.Generic;
namespace GravityCrypto.Client.Functions;
public class Functions
{
    /// <summary>
    /// The alphabet string containing all uppercase letters from A to Z.
    /// </summary>
    public const string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    /// <summary>
    /// The current string expression that is being manipulated.
    /// </summary>
    private string string_expression;
    
    /// <summary>
    /// The current string expression that is being manipulated.
    /// </summary>
    public string StringExpression
    {
        get => string_expression;
        set
        {
            string_expression = value;
            UpdateHistory();
        }
    }

    /// <summary>
    /// History of previous string expressions.
    /// </summary>
    private readonly string[] string_expression_history;
    
    /// <summary>
    /// The history of previous string expressions.
    /// </summary>
    public string[] StringExpressionHistory { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Functions"/> class.
    /// </summary>
    /// <param name="expression">The string expression to be processed.</param>
    public Functions(string expression)
    {
        string_expression = expression;
        string_expression_history = new string[100];
    }

    /// <summary>
    /// Prompts the user to input a string of numbers.
    /// </summary>
    public void Input()
    {
        Console.WriteLine("Enter a string of numbers: ");
        NewExpression(Console.ReadLine() ?? string.Empty);
    }

    /// <summary>
    /// Updates the current string expression and stores the previous expression in history.
    /// </summary>
    /// <param name="expression">The new string expression to be processed.</param>
    public void NewExpression(string expression)
    {
        StringExpression = expression;
    }

    /// <summary>
    /// Decodes a Polybius cipher using the provided cipher text and Polybius square.
    /// </summary>
    /// <param name="cipherText">The encoded cipher text string.</param>
    /// <param name="polybiusSquare">The Polybius square string used for decoding.</param>
    /// <returns>The decoded string.</returns>
    public string DecodePolybius(string cipherText, string polybiusSquare)
    {
        Dictionary<string, char> polybiusMap = DecodePolybiusHelper(polybiusSquare);
        StringBuilder decodedText = new StringBuilder();
        
        for (int i = 0; i < cipherText.Length; i += 2)
        {
            string pair = cipherText.Substring(i, 2);
            if (polybiusMap.ContainsKey(pair))
            {
                decodedText.Append(polybiusMap[pair]);
            }
        }
        
        return decodedText.ToString();
    }
    
    /// <summary>
    /// Decodes a binary string into a readable string.
    /// </summary>
    /// <param name="binaryString">The binary string to decode.</param>
    /// <param name="length">The length of each binary byte.</param>
    /// <returns>The decoded readable string.</returns>
    public string DecodeBinaryString(string binaryString, int length = 8)
    {
        if (string.IsNullOrEmpty(binaryString) || binaryString.Length % length != 0)
        {
            throw new ArgumentException("Invalid binary string or length.");
        }

        var result = new StringBuilder();
        for (int i = 0; i < binaryString.Length; i += length)
        {
            string byteString = binaryString.Substring(i, length);
            byte decodedByte = System.Convert.ToByte(byteString, 2);
            result.Append((char)decodedByte);
        }

        return result.ToString();
    }

    /// <summary>
    /// Handles user input for the binary decoding process.
    /// </summary>
    public void HandleUserInputForBinaryDecoding()
    {
        Console.WriteLine("Enter the binary string to decode:");
        string binaryString = string_expression;

        Console.WriteLine("Enter the length of each binary byte (default is 8):");
        string lengthInput = Console.ReadLine() ?? string.Empty;

        int length = 8; // Default length is 8
        if (!string.IsNullOrEmpty(lengthInput) && int.TryParse(lengthInput, out int parsedLength))
        {
            length = parsedLength;
        }

        // If length is less than 8, pad each binary byte with leading zeros
        if (length < 8)
        {
            binaryString = PadBinaryString(binaryString, length);
        }

        try
        {
            string decodedString = DecodeBinaryString(binaryString, 8); // Always decode as if each byte is 8 bits long
            Console.WriteLine("Decoded string: " + decodedString);
            NewExpression(decodedString);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error decoding binary string: " + ex.Message);
        }
    }

    /// <summary>
    /// Pads each binary byte in the string with leading zeros to make it 8 bits long.
    /// </summary>
    /// <param name="binaryString">The binary string to pad.</param>
    /// <param name="length">The current length of each binary byte.</param>
    /// <returns>The padded binary string.</returns>
    private string PadBinaryString(string binaryString, int length)
    {
        var paddedBinaryString = new StringBuilder();

        for (int i = 0; i < binaryString.Length; i += length)
        {
            string byteString = binaryString.Substring(i, length);
            string paddedByteString = byteString.PadLeft(8, '0'); // Pad with leading zeros to make it 8 bits long
            paddedBinaryString.Append(paddedByteString);
        }

        return paddedBinaryString.ToString();
    }


    /// <summary>
    /// Creates a dictionary mapping the coordinate pairs to letters based on the provided Polybius square.
    /// </summary>
    /// <param name="polybiusSquare">The Polybius square string.</param>
    /// <returns>A dictionary mapping coordinate pairs to corresponding letters.</returns>
    public Dictionary<string, char> DecodePolybiusHelper(string polybiusSquare)
    {
        Dictionary<string, char> polybiusMap = new Dictionary<string, char>();
        
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                string key = (i + 1).ToString() + (j + 1).ToString();
                polybiusMap[key] = polybiusSquare[i * 5 + j];
            }
        }

        return polybiusMap;
    }

    /// <summary>
    /// Handles user input for the Polybius decoding process and applies the Polybius square.
    /// </summary>
    public void DecodePolybiusInputHelper()
    {
        Console.WriteLine("Do you want to use a specific Polybius square? (Y/N)");
        string useSpecificSquare = Console.ReadLine() ?? string.Empty;
        
        string polybiusSquare = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
        if (useSpecificSquare.ToUpper() == "Y")
        {
            Console.WriteLine("Enter the Polybius square: ");
            polybiusSquare = Console.ReadLine() ?? string.Empty;
        }
        
        string result = DecodePolybius(string_expression, polybiusSquare);
        Console.WriteLine(result);
        NewExpression(result);
    }

    /// <summary>
    /// Updates the history of string expressions by adding the current expression to the top of the history,
    /// only if it's different from the most recent entry.
    /// </summary>
    public void UpdateHistory()
    {
        // Check if the current expression is different from the most recent history entry
        if (string_expression != string_expression_history[0])
        {
            for (int i = string_expression_history.Length - 1; i > 0; i--)
            {
                string_expression_history[i] = string_expression_history[i - 1];
            }
            string_expression_history[0] = string_expression;
        }
    }

    /// <summary>
    /// Reverts the current string expression to the most recent previous expression in history.
    /// </summary>
    public void Revert()
    {
        // Check if there's a previous expression to revert to
        if (string_expression_history[1] != null)
        {
            string_expression = string_expression_history[1];
            for (int i = 1; i < string_expression_history.Length - 1; i++)
            {
                string_expression_history[i] = string_expression_history[i + 1];
            }
            string_expression_history[string_expression_history.Length - 1] = null!;
            Console.WriteLine(string_expression);
        }
        else
        {
            Console.WriteLine("No previous expression to revert to.");
        }
    }

    /// <summary>
    /// Converts a string of numbers separated by dashes into corresponding letters.
    /// </summary>
    /// <param name="expression">The string expression containing numbers.</param>
    public string ConvertA1Z26(string expression)
    {
        StringBuilder result = new StringBuilder();
        string[] words = expression.Split(' ');
        foreach (string word in words)
        {
            string[] digits = word.Split('-');
            foreach (string digit in digits)
            {
                if (int.TryParse(digit, out int number))
                {
                    if (number > 0 && number < 27)
                    {
                        result.Append(alphabet[number - 1]);
                    }
                    else
                    {
                        result.Append(number);
                    }
                }
                else
                {
                    result.Append(digit);
                }
            }
            result.Append(' ');
        }
        Console.WriteLine(result);
        NewExpression(result.ToString());
        return result.ToString();
    }

    /// <summary>
    /// Shifts each letter in the current string expression by the position of the first letter.
    /// </summary>
    public void ShiftByFirstLetter()
    {
        Console.WriteLine("Enter the first letter: ");
        char firstLetter = Console.ReadKey().KeyChar;
        try
        {
            int shift = alphabet.IndexOf(char.ToUpper(firstLetter));
            Shift(shift);
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid first letter");
        }
    }

    /// <summary>
    /// Shifts each letter in the current string expression by a specified number of positions in the alphabet.
    /// </summary>
    /// <param name="shift">The number of positions to shift.</param>
    public void Shift(int shift)
    {
        StringBuilder result = new StringBuilder();
        foreach (char c in string_expression)
        {
            if (char.IsLetter(c))
            {
                int index = alphabet.IndexOf(char.ToUpper(c));
                index = (index + shift) % alphabet.Length;
                if (index < 0)
                {
                    index += alphabet.Length;
                }
                result.Append(char.IsLower(c) ? char.ToLower(alphabet[index]) : alphabet[index]);
            }
            else
            {
                result.Append(c);
            }
        }
        Console.WriteLine(result);
        NewExpression(result.ToString());
    }

    /// <summary>
    /// Reverses the characters in the current string expression.
    /// </summary>
    /// <param name="expression">The string expression to reverse.</param>
    public void Reverse(string expression)
    {
        char[] charArray = expression.ToCharArray();
        Array.Reverse(charArray);
        Console.WriteLine(new string(charArray));
        NewExpression(new string(charArray));
    }

    /// <summary>
    /// Reverses each letter in the current string expression based on its position in the alphabet.
    /// </summary>
    public void ReverseAlphabet()
    {
        StringBuilder result = new StringBuilder();
        foreach (char c in string_expression)
        {
            if (char.IsLetter(c))
            {
                int index = alphabet.IndexOf(char.ToUpper(c));
                index = alphabet.Length - index - 1;
                result.Append(char.IsLower(c) ? char.ToLower(alphabet[index]) : alphabet[index]);
            }
            else
            {
                result.Append(c);
            }
        }
        Console.WriteLine(result);
        NewExpression(result.ToString());
    }

    /// <summary>
    /// Prompts the user to enter a key and applies the Vigenère cipher to the current string expression.
    /// </summary>
    public void VigenèreHelper()
    {
        Console.WriteLine("Enter the key: ");
        string key = Console.ReadLine() ?? string.Empty;
        Vigenère(key);
    }

    /// <summary>
    /// Applies the Vigenère cipher to the current string expression using the provided key.
    /// </summary>
    /// <param name="key">The key for the Vigenère cipher.</param>
    public void Vigenère(string key)
    {
        StringBuilder result = new StringBuilder();
        int keypos = 0;
        while (key.Length < string_expression.Length)
        {
            key += key;
        }
    
        for (int i = 0; i < string_expression.Length && i < key.Length; i++)
        {
            if (!char.IsLetter(string_expression[i]))
            {
                result.Append(string_expression[i]);
                continue;
            }
            int x = (string_expression[i] - key[keypos] + 26) % 26;
            x += 'A';
            result.Append((char)(x));
            keypos++;
        }
        Console.WriteLine(result);
        NewExpression(result.ToString());
    }

    /// <summary>
    /// Combines multiple operations: Convert to alphabet, reverse alphabet, and shift letters by -3.
    /// </summary>
    public void Combined()
    {
        ConvertA1Z26(string_expression);
        ReverseAlphabet();
        Shift(-3);
    }
}