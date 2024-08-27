using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
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

namespace FuelCellStoreTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // list of all products
        List<Item> products;

        // maps all specifications to their type (num or string)
        Dictionary<string, string> specParams;

        public MainWindow()
        {
            InitializeComponent();

            // init ui items
            OperatorBox.Items.Add("=");
            OperatorBox.Items.Add(">");
            OperatorBox.Items.Add("<");

            // load database
            products = new List<Item>();
            specParams = new Dictionary<string, string>();
            LoadJson(products, specParams);

            // update listview
            UpdateProductDisplay();
        }

        public void LoadJson(List<Item> prod, Dictionary<string, string> param)
        {
            // change path to update_items.py output path
            using (StreamReader r = File.OpenText(@"C:\Users\steven\Documents\FuelCellStore\data\raw_dump.json"))
            {
                string json = r.ReadToEnd();
                dynamic items = JsonConvert.DeserializeObject(json);

                // convert dynamic into typed for convenience
                foreach (var item in items)
                {
                    Item curr = new Item();
                    curr.name = item.name;
                    curr.link = item.link;
                    curr.stock = item.stock;
                    curr.spec = DynToDict(item.spec);

                    prod.Add(curr);

                    foreach (string key in curr.spec.Keys)
                    {
                        if (!param.ContainsKey(key))
                        {
                            param.Add(key, curr.spec[key].Any(char.IsDigit) ? "num" : "string");
                        }
                    }
                }
            }
        }

        // dynamic to dictionary helper func
        public Dictionary<string, string> DynToDict(dynamic dynObj)
        {
            var dictionary = new Dictionary<string, string>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynObj))
            {
                string obj = propertyDescriptor.GetValue(dynObj);
                dictionary.Add(propertyDescriptor.Name, obj);
            }
            return dictionary;
        }

        // shop item
        public class Item
        {
            public string name;
            public string link;
            public bool stock;
            public Dictionary<string, string> spec;
        }

        // range parse helper func
        public float[] ParseNumerical(string str)
        {
            MatchCollection matches = Regex.Matches(str, @"[-+]?(?:\d*\.*\d+)");

            List<float> nums = new List<float>();

            foreach (Match match in matches)
            {
                nums.Add(float.Parse(match.Value));
            }

            if (nums.Count >= 1)
            {
                string op = str.Substring(0, 1);

                if (op.Equals("<"))
                {
                    return [0, nums[0]];
                }
                else if (op.Equals(">")) 
                {
                    return [nums[0], float.MaxValue];
                }
                else
                {
                    if (nums.Count >= 2 &&
                        (str.Contains("-") || str.Contains("to")))
                    {
                        return [nums[0], nums[1]];
                    }
                    else
                    {
                        return [nums[0], nums[0]];
                    }
                }
            }

            return [0, float.MaxValue];
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateResults();
            UpdateProductDisplay();
        }

        public void UpdateProductDisplay()
        {
            ProductList.Items.Clear();
            foreach (Item prod in products)
            {
                ProductList.Items.Add(prod.name);
            }
        }

        public void UpdateResults()
        {
            string propName = NameBox.Text;
            string propValString = ValueBox.Text;
            string operatorValue = OperatorBox.Text;
            float propValFloat = 0;
            bool valIsFloat = float.TryParse(propValString, out propValFloat);

            string specMatch = "";

            foreach (string param in specParams.Keys)
            {
                if (param.ToUpper().Contains(propName.ToUpper()) && (specMatch.Length == 0 || param.Length < specMatch.Length))
                {
                    specMatch = param;
                }
            }

            if (specMatch.Length > 0)
            {
                string valueType = specParams[specMatch];

                if (valueType.Equals("string"))
                {
                    foreach (Item prod in products.ToList())
                    {
                        if (prod.spec.ContainsKey(specMatch) && prod.spec[specMatch].ToUpper().Contains(propValString.ToUpper())) 
                        {
                            //System.Diagnostics.Debug.WriteLine(prod.spec[specMatch]);
                        } 
                        else
                        {
                            products.Remove(prod);
                        }
                    }
                }
                else
                {
                    if (!valIsFloat)
                        return;

                    foreach (Item prod in products.ToList())
                    {
                        if (operatorValue.Equals("<"))
                        {
                            if (prod.spec.ContainsKey(specMatch)
                                && ParseNumerical(prod.spec[specMatch])[1] <= propValFloat)
                            {
                                //System.Diagnostics.Debug.WriteLine(prod.spec[specMatch]);
                            }
                            else
                            {
                                products.Remove(prod);
                            }
                        }
                        else if (operatorValue.Equals(">"))
                        {
                            if (prod.spec.ContainsKey(specMatch)
                                && ParseNumerical(prod.spec[specMatch])[0] >= propValFloat)
                            {
                                //System.Diagnostics.Debug.WriteLine(prod.spec[specMatch]);
                            }
                            else
                            {
                                products.Remove(prod);
                            }
                        }
                        else
                        {
                            if (prod.spec.ContainsKey(specMatch) 
                                && ParseNumerical(prod.spec[specMatch])[0] <= propValFloat
                                && ParseNumerical(prod.spec[specMatch])[1] >= propValFloat)
                            {
                                //System.Diagnostics.Debug.WriteLine(prod.spec[specMatch]);
                            }
                            else
                            {
                                products.Remove(prod);
                            }
                        }
                    }
                }
            }
        }
    }

}