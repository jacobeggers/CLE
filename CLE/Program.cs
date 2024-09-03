using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;

public class Program
{

    public static void Main(string[] args)
    {
        bool isFirstIteration = true;

        bool saved = false;

        char[][] content = new char[1][];

        string beginDisplayString = $"=-=-=-=-=-(Saved: {saved} | File Type: .txt | File Name: test | Size: {0}kb)-=-=-=-=-=";
        string endDisplayString = "";

        for (int i = 0; i < beginDisplayString.Length; i++)
        {
            endDisplayString += '=';
        }
        beginDisplayString += '\n';
        endDisplayString += '\n';

        int indentLength = 0;

        int cursorX = 0;
        int cursorY = 0;

        int consoleStartingY = 0;
        int finalConsoleLength = 10;
        int consoleLength = finalConsoleLength;

        ConsoleKey key = ConsoleKey.NoName;
        char keyChar = '\0';




        for (int i = 0; i < content.Length; i++)
        {
            content[i] = new char[1];
            for (int j = 0; j < content[i].Length; j++)
            {
                content[i][j] = '\0';
            }
        }

        while (true)
        {
            isFirstIteration = resetScreen(isFirstIteration, content, consoleStartingY, consoleLength, indentLength, beginDisplayString, endDisplayString);

            if (key != ConsoleKey.NoName )
            {
                if (key == ConsoleKey.LeftArrow && cursorX > 0)
                {
                    cursorX--;
                }
                else if (key == ConsoleKey.RightArrow && cursorX < content[cursorY].Length - 1)
                {
                    cursorX++;
                }
                else if (key == ConsoleKey.UpArrow && cursorY > 0)
                {
                    cursorY--;
                    if (cursorY < consoleStartingY)
                    {
                        consoleStartingY--;
                    }
                }
                else if (key == ConsoleKey.DownArrow && cursorY < content.Length - 1)
                {
                    cursorY++;
                    if (cursorY >= consoleStartingY + consoleLength)
                    {
                        consoleStartingY++;
                    }
                }
                else if (keyChar != '\0')
                {
                    if (keyChar == '\b')
                    {
                        if (cursorX > 0)
                        {
                            content[cursorY] = removeChar(cursorX, content[cursorY]);
                            cursorX--;
                        } else if (cursorY > 0)
                        {
                            cursorX = content[cursorY - 1].Length - 1;
                            for (int i = 0; i < content[cursorY].Length - 1; i++)
                            {
                                content[cursorY - 1] = insertChar(content[cursorY - 1].Length - 1, content[cursorY][i], content[cursorY - 1]);
                            }
                            content = removeRow(cursorY, content);
                            cursorY--;
                            if (consoleStartingY > 0)
                            {
                                consoleStartingY--;
                            }
                        }
                    }
                    else if (key == ConsoleKey.Enter)
                    {
                        content = insertRow(cursorY, content);
                        cursorY++;
                        cursorX = 0;
                        if (cursorY >= consoleStartingY + consoleLength)
                        {
                            consoleStartingY++;
                        }
                    }
                    else
                    {
                        content[cursorY] = insertChar(cursorX, keyChar, content[cursorY]);
                        cursorX++;
                    }
                }
            }

            string finalString = "";

            finalString += beginDisplayString;
            
            if (Console.WindowHeight - 3 < finalConsoleLength)
            {
                consoleLength = Console.WindowHeight - 3;
                if (cursorY > consoleStartingY + consoleLength || cursorY < consoleStartingY)
                {
                    consoleStartingY = cursorY;
                }
            } else
            {
                consoleLength = finalConsoleLength;
            }

            int numberIndent = 0;
            int endLine = (content.Length < consoleStartingY + consoleLength ? content.Length : consoleStartingY + consoleLength) - 1;
            while (endLine > 0)
            {
                endLine /= 10;
                numberIndent++;
            }
            indentLength = numberIndent + 5;

            for (int i = consoleStartingY; i < (content.Length < consoleStartingY + consoleLength ? content.Length : consoleStartingY + consoleLength); i++)
            {
                finalString += ' ';
                
                int numberLength = 0;
                int index = i;
                while (index > 0)
                {
                    index /= 10;
                    numberLength++;
                }
                for (int j = 0; j < (i != 0 ? numberIndent - numberLength : numberIndent - 1); j++)
                {
                    finalString += ' ';
                }
                
                finalString += $"{i} : ";
                for (int j = 0; j < content[i].Length; j++)
                {
                    if (j == cursorX && i == cursorY)
                    {
                        finalString += "█";
                    }
                    // else if (content[i][j] == ' ')
                    // {
                    //     finalString += '·';
                    // }
                    else if (content[i][j] != '\0')
                    {
                        finalString += content[i][j];
                    }
                }
                finalString += "\n";
            }

            finalString += endDisplayString;

            Console.Write(finalString);
            ConsoleKeyInfo info = Console.ReadKey();
            key = info.Key;
            keyChar = info.KeyChar;
            Console.Write($"\b \b");

        }
    }

    public static char[] insertChar(int index, char c, char[] content)
    {
        char[] newContent = new char[content.Length + 1];
        
        int ind = 0;
        for (int i = 0; i < content.Length; i++)
        {
            if (i == index)
            {
                newContent[ind] = c;
                ind++;
            }
            
            newContent[ind] = content[i];
            ind++;
        }

        return newContent;
    }

    public static char[] removeChar(int index, char[] content)
    {
        char[] newContent = new char[content.Length - 1];

        int ind = 0;
        for (int i = 0; i < content.Length; i++)
        {
            if (i == index - 1)
            {
                i++;
            }

            
            newContent[ind] = content[i];
            ind++;
        }

        return newContent;
    }

    public static char[][] insertRow(int index, char[][] content)
    {
        char[][] newContent = new char[content.Length + 1][];

        int ind = 0;
        if (index == content.Length - 1)
        {
            newContent[content.Length] = new char[1] { '\0' };
        }
        for (int i = 0; i < content.Length; i++)
        {
            if (i == index + 1)
            {
                newContent[ind] = new char[1] { '\0' };
                ind++;
            }

            newContent[ind] = content[i];
            ind++;
        }

        return newContent;
    }

    public static char[][] removeRow(int index, char[][] content)
    {
        char[][] newContent = new char[content.Length - 1][];

        int ind = 0;
        for (int i = 0; i < content.Length; i++)
        {
            if (index < content.Length - 1)
            {
                if (i == index)
                {
                    i++;
                }
                newContent[ind] = content[i];
                ind++;
            } else
            {
                if (i < content.Length - 1)
                {
                    newContent[i] = content[i];
                }
            }
        }

        return newContent;
    }

    public static bool resetScreen(bool isFirstIteration, char[][] content, int y, int length, int indentLength, string beginDisplayString, string endDisplayString)
    {
        if (!isFirstIteration)
        {

            // Console.SetCursorPosition(0, Console.CursorTop - 1);
            // for (int i = y; i < (content.Length < y + length ? content.Length : y + length); i++)
            // {
            //     Console.SetCursorPosition(0, Console.CursorTop - 1);
            // }
            // Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.SetCursorPosition(0, Console.CursorTop - (content.Length < y + length ? content.Length : length) - 2);

            string resetString = "";

            for (int j = 0; j < beginDisplayString.Length; j++)
            {
                resetString += " ";
            }

            resetString += "\n";

            for (int i = y; i < (content.Length < y + length ? content.Length : y + length); i++)
            {
                for (int j = 0; j < content[i].Length + indentLength; j++)
                {
                    resetString += " ";
                }
                resetString += "\n";
            }

            for (int j = 0; j < endDisplayString.Length; j++)
            {
                resetString += " ";
            }

            resetString += "\n";

            Console.Write(resetString);

            Console.SetCursorPosition(0, Console.CursorTop - 1);
            for (int i = y; i < (content.Length < y + length ? content.Length : y + length); i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
            Console.SetCursorPosition(0, Console.CursorTop - 1);

        }
        else
        {
            isFirstIteration = false;
        }
        return isFirstIteration;
    }
}

