using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Algida_Ice_Cream
{
    internal class IceCreamHelper : Window
    {
        internal class NumericUpDown  // Define a nested class for handling NumericUpDown functionality
        {
            public static void IncreaseValue(TextBox textBox) // Increase the value of the TextBox (up to a limit)
            {
                int currentValue = int.Parse(textBox.Text);
                if (currentValue < 100)  // Limit the value to 100 if necessary
                {
                    currentValue++;
                    textBox.Text = currentValue.ToString();

                }
            }

            public static void DecreaseValue(TextBox textBox) // Decrease the value of the TextBox (down to 0)
            {
                int currentValue = int.Parse(textBox.Text);
                if (currentValue > 0)
                {
                    currentValue--;
                    textBox.Text = currentValue.ToString();
                }
            }
        }

        public class Order // Define a class for representing an Order
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string TypeOfMilk { get; set; }
            public string Extra { get; set; }
            public string UnitPrice { get; set; }
            public string UnitCalori { get; set; }
            public string Amount { get; set; }
            public string TotalPrice { get; set; }
            public string SelectedFlavors { get; internal set; }
        }

        // Update the properties of UI elements based on input validity
        public static void UpdateElementProperties(TextBox addressTextBox , TextBox nameTextBox, UIElement[] elementsToEnable)
        {
            bool isInputValid =false;

            if (addressTextBox.Text != string.Empty && nameTextBox.Text != string.Empty)
            {
                isInputValid = true;
            }

            foreach (var element in elementsToEnable)
            {
                element.IsEnabled = isInputValid;
            }
        }


        // Capitalize the first letter of a string
        public static string CapitalizeFirstLetter(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                return char.ToUpper(input[0]) + input.Substring(1);
            }
            return input; // Return the input as is if it's empty or null
        }

        public static string GetSelectedMilk(StackPanel milkStackPanel) // Get the selected type of milk from a StackPanel
        {
            string selectedMilk = string.Empty;

            foreach (RadioButton radioButton in milkStackPanel.Children)
            {
                if (radioButton.IsChecked == true)
                {
                    selectedMilk = radioButton.Content.ToString();
                    break;
                }
            }

            return selectedMilk;
        }
        public static List<string> GetSelectedFlavors(StackPanel flavorStackPanel) // Get a list of selected flavors from a StackPanel
        {
            List<string> selectedFlavors = new List<string>();

            foreach (StackPanel stackPanel in flavorStackPanel.Children)
            {
                CheckBox checkBox = stackPanel.Children.OfType<CheckBox>().FirstOrDefault();
                TextBox textBox = stackPanel.Children.OfType<TextBox>().FirstOrDefault();

                if (checkBox.IsChecked == true)
                {
                    string flavorName = checkBox.Content.ToString();
                    int quantity = int.Parse(textBox.Text);

                    if (quantity > 0)
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            selectedFlavors.Add(flavorName);
                        }
                    }
                }
            }

            return selectedFlavors;
        }

        public static string GetSelectedExtra(StackPanel StackExtra) // Get the selected extra from a StackPanel
        {
            string selectedExtra = string.Empty;

            foreach (RadioButton radioButton in StackExtra.Children)
            {
                if (radioButton.IsChecked == true)
                {
                    selectedExtra = radioButton.Content.ToString();
                    break;
                }
            }

            return selectedExtra;
        }


        



        public static void ClearAdditions(StackPanel stackPanel) // Clear the checkboxes and textboxes related to flavor additions
        {
            foreach (var child in stackPanel.Children)
            {
                if (child is StackPanel additionStackPanel)
                {
                    foreach (var innerChild in additionStackPanel.Children)
                    {
                        if (innerChild is TextBox textBox)
                        {
                            textBox.Text = "0";
                        }
                        else if (innerChild is CheckBox checkBox)
                        {
                            checkBox.IsChecked = false;
                        }
                    }
                }
            }
        }


        public static void ClearExtra(StackPanel stackPanel) // Clear the selected extra options
        {
            foreach (var child in stackPanel.Children)
            {
                if (child is RadioButton radioButton)
                {
                    radioButton.IsChecked = false;
                }
            }
        }

        public static void ClearTypeOfMilk(StackPanel stackPanel) // Clear the selected type of milk
        {
            foreach (var child in stackPanel.Children)
            {
                if (child is RadioButton radioButton)
                {
                    radioButton.IsChecked = false;
                }
            }
        }


        public static void ClearTextBoxes(params TextBox[] textBoxes) // Clear the content of specified textboxes
        {
            foreach (var textBox in textBoxes)
            {
                textBox.Text = "";
            }

        }

        // Update the visibility of flavor images based on checkbox state and quantity
        public static void UpdateImageVisibility(CheckBox checkBox, TextBox textBox, Image image)
        {
            int quantity = Convert.ToInt32(textBox.Text);
            bool isChecked = checkBox.IsChecked == true;

            image.Visibility = isChecked && quantity > 0 ? Visibility.Visible : Visibility.Hidden;
        }




        //=========================
    }
}
