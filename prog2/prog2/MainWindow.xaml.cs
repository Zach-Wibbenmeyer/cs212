/* CS 212 - Data Structures and Algorithms
 * Professor: Harry Plantinga
 * Author: Zach Wibbenmeyer
 * Date: October 14, 2016
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace prog2
{
    public partial class MainWindow : Window
    {
        // input file
        private string input;
        // input file broken into array of words          
        private string[] words;
        // number of words to babble
        private int wordCount = 200;
        //order variable
        private int order = 0;
        //allows random selection of numbers
        private Random rand_num = new Random();
        //creates a hash table
        private static Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>(); 

        public MainWindow()
        {
            InitializeComponent();
        }

        //DIctates what happens when the Load button is clicked
        private void loadButton_Click(object sender, RoutedEventArgs e) 
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            // Default file name
            ofd.FileName = "Sample"; 
            // Default file extension
            ofd.DefaultExt = ".txt"; 
            // Filter files by extension
            ofd.Filter = "Text documents (.txt)|*.txt"; 

            // Show open file dialog box
            if ((bool)ofd.ShowDialog())
            {
                textBlock1.Text = "Loading file " + ofd.FileName + "\n";
                // read file
                input = System.IO.File.ReadAllText(ofd.FileName);  
                // split into array of words
                words = Regex.Split(input, @"\s+");       

            }
        }

        private void analyzeInput(int order_1)
        {
            if (order_1 > 0)
            {
                //Displays what order the text is being analyzed at
                MessageBox.Show("Analyzing at order: " + order_1); 

            }

            order = order_1;
        }

        /* babbleButton_Click() - computes order 0 and order 1 statistics
         * @param: sender (type -> object), e (type -> RoutedEventArgs)
         * @return: array of strings
         * Precondition: an order must be selected
         */
        private void babbleButton_Click(object sender, RoutedEventArgs e)
        {
            if (order == 0)
            {
                orderZero();
            }
            else if (order == 1)
            {
                orderOne();
            }
        }

        /* orderOne() - Computes order one statistics for Babble
         * @param: None
         * @return: an array of strings
         */
        private void orderOne()
        {
            //Loops through the entire array of words
            for (int i = 0; i < words.Length - 1; i++) 
            {
                if (!hashTable.ContainsKey(words[i]))
                    //adds any new word into the hash tables
                    hashTable.Add(words[i], new ArrayList());
                //adds the word after i to the hash table                  
                hashTable[words[i]].Add(words[i + 1]); 
            }

            textBlock1.Text = words[0] + " ";
            string current_Word = words[0];
            string next_Word = "";

            for (int i = 0; i < wordCount - 1; i++)
            {
                //When the hash table doesn't contain the current word
                if (!hashTable.ContainsKey(current_Word))
                    //resets to position 0 in hash table 
                    next_Word = words[0]; 
                else
                    next_Word = (hashTable[current_Word][rand_num.Next(0, hashTable[current_Word].Count - 1)]).ToString();
                textBlock1.Text += next_Word + " ";
                current_Word = next_Word;
            }
            //Displays how many words are in the file that is read
            MessageBox.Show("There are " + words.Length + " words.");
            //Displays how many unique words there are in the file that is read
            MessageBox.Show("There are " + hashTable.Count() + " unique words."); 
        }

        /* orderZero() - Computes order zero statistics for Babble
         * @param: None
         * @return: array of strings
         */
        private void orderZero()
        {
            for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
                //prints out the contents of a file in the original order
                textBlock1.Text += " " + words[i];
            //Displays how many words are in the file that is read
            MessageBox.Show("There are " + words.Length + " words."); 
        }

        private void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            analyzeInput(orderComboBox.SelectedIndex);
        }
    }
}
