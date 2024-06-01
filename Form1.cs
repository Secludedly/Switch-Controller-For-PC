using Newtonsoft.Json;
using SysBot.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SysbotMacro
{
    public partial class Form1 : Form
    {
        private bool canSaveData = true;
        private List<Bot> bots = new List<Bot>();
        private bool live;
        private List<Dictionary<string, object>> ipDict = new List<Dictionary<string, object>>();
        private List<Dictionary<string, object>> macroDict = new List<Dictionary<string, object>>();


        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void SaveData()
        {
            if (!canSaveData)
            {
                return;
            }


            ipDict.Clear();
            foreach (ListViewItem item in ipListView.Items)
            {
                var itemData = new Dictionary<string, object>
        {
            { "IsChecked", item.Checked },
            { "SwitchName", item.SubItems[1].Text },
            { "IPAddress", item.SubItems[2].Text }
        };
                ipDict.Add(itemData);
            }

            File.WriteAllText("ipListView.json", JsonConvert.SerializeObject(ipDict));

            macroDict.Clear();
            foreach (ListViewItem item in macroListView.Items)
            {
                var itemData = new Dictionary<string, object>
        {
            { "Name", item.Text },
            { "Macro", item.SubItems[1].Text }
        };
                macroDict.Add(itemData);
            }

            File.WriteAllText("macroListView.json", JsonConvert.SerializeObject(macroDict));

        }

        private void LoadData()
        {
            if (File.Exists("macroListView.json"))
            {
                var macroListViewDataJson = File.ReadAllText("macroListView.json");
                var macroListViewData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(macroListViewDataJson);

                macroListView.Items.Clear();
                foreach (var itemData in macroListViewData)
                {
                    var newItem = new ListViewItem
                    {
                        Text = itemData["Name"].ToString()
                    };
                    newItem.SubItems.Add(itemData["Macro"].ToString());
                    macroListView.Items.Add(newItem);
                }
            }

            if (File.Exists("ipListView.json"))
            {
                var ipListViewDataJson = File.ReadAllText("ipListView.json");
                var ipListViewData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(ipListViewDataJson);

                ipListView.Items.Clear();
                foreach (var itemData in ipListViewData)
                {
                    var newItem = new ListViewItem
                    {
                        Checked = (bool)itemData["IsChecked"]
                    };
                    newItem.SubItems.Add(itemData["SwitchName"].ToString());
                    newItem.SubItems.Add(itemData["IPAddress"].ToString());
                    ipListView.Items.Add(newItem);
                }
            }
        }

        private void InitializeBots()
        {
            bots.Clear();

            if (ipListView.CheckedItems.Count < 1)
            {
                UpdateLogger("No IP selected");
                return;
            }

            foreach (ListViewItem item in ipListView.CheckedItems)
            {
                string ip = item.SubItems[2].Text;

                var config = new SwitchConnectionConfig
                {
                    IP = ip,
                    Port = 6000,
                    Protocol = SwitchProtocol.WiFi
                };

                var bot = new Bot(config);
                bots.Add(bot);
            }
        }

        private void AppendToLastNumberString(string appendText)
        {
            string text = textBox1.Text.TrimEnd();
            string[] splitText = text.Split(' ');

            if (Regex.IsMatch(splitText[splitText.Length - 1], @"\d"))
            {
                Console.WriteLine("Not a button");
            }
            else
            {
                splitText[splitText.Length - 1] += appendText;
            }
            textBox1.Text = string.Join(" ", splitText);
            textBox1.Text += " ";
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                ListViewItem newItem = new ListViewItem("");
                newItem.Text = macroNameTB.Text;
                newItem.SubItems.Add(textBox1.Text);
                macroListView.Items.Add(newItem);
                SaveData();
            }

        }


        #region Joycon Button inputs
        private async void LeftbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Left");
            }
            else
            {
                textBox1.AppendText("Left ");
            }
        }

        private async void RightbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Right");
            }
            else
            {
                textBox1.AppendText("Right ");
            }

        }

        private async void YbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Y");
            }
            else
            {
                textBox1.AppendText("Y ");
            }
        }

        private async void XbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("X");
            }
            else
            {
                textBox1.AppendText("X ");
            }
        }

        private async void AaBbutton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("A");
            }
            else
            {
                textBox1.AppendText("A ");
            }
        }

        private async void BbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("B");
            }
            else
            {
                textBox1.AppendText("B ");
            }

        }

        private async void RtsbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("RStick");
            }
            else
            {
                textBox1.AppendText("RTS ");
            }
        }

        private async void HbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Home");
            }
            else
            {
                textBox1.AppendText("H ");
            }

        }
        private async void SsbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Capture");
            }
            else
            {
                textBox1.AppendText("SS ");
            }
        }

        private async void LtsbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("LStick");
            }
            else
            {
                textBox1.AppendText("LTS ");
            }

        }

        private async void DownbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Down");
            }
            else
            {
                textBox1.AppendText("Down ");
            }

        }

        private async void UpbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Up");
            }
            else
            {
                textBox1.AppendText("Up ");
            }

        }

        private async void RbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Right");
            }
            else
            {
                textBox1.AppendText("R ");
            }

        }

        private async void ZrbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("ZR");
            }
            else
            {
                textBox1.AppendText("ZR ");
            }

        }

        private async void ZlbButton_Click_1(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("ZL");
            }
            else
            {
                textBox1.AppendText("ZL ");
            }
        }

        private async void LbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("L");
            }
            else
            {
                textBox1.AppendText("L ");
            }
        }

        private async void ZlbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("ZL");
            }
            else
            {
                textBox1.AppendText("ZL ");
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            SaveData();
            try
            {
                textBox1.Text = macroListView.SelectedItems[0].SubItems[1].Text;
            }
            catch
            {
                UpdateLogger("No macro selected");
            }

        }

        private async void PlusbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Plus");
            }
            else
            {
                textBox1.AppendText("+ ");
            }

        }

        private async void MinusbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Minus");
            }
            else
            {
                textBox1.AppendText("- ");
            }
        }

        #endregion Button

        private void HoldButton_Click(object sender, EventArgs e)
        {
            /*
            string timenumdirty = timerInputField.Text;
            string digitsOnly = Regex.Replace(timenumdirty, @"\D", "");
            digitsOnly = digitsOnly + " ";
            textBox1.AppendText(digitsOnly);
            */
        }
        private void TimerInputField_TextChanged(object sender, EventArgs e)
        {

        }

        private void DelayButton_Click(object sender, EventArgs e)
        {
            if (delayInputField.Text != "")
            {
                string delaynumdirty = delayInputField.Text;
                string delaydigitsOnly = Regex.Replace(delaynumdirty, @"\D", "");
                delaydigitsOnly = "d" + delaydigitsOnly + " ";
                textBox1.AppendText(delaydigitsOnly);
            }
        }

        private void AddIpButton_Click(object sender, EventArgs e)
        {
            string ipText = ipTextField.Text;
            string switchName = switchNameTB.Text;

            if (string.IsNullOrEmpty(switchName))
            {
                UpdateLogger("Add a name for the switch");
                return;
            }

            if (!string.IsNullOrEmpty(ipText))
            {
                if (IPAddress.TryParse(ipText, out IPAddress address))
                {
                    ListViewItem existingItem = null;
                    foreach (ListViewItem item in ipListView.Items)
                    {
                        if (item.SubItems[1].Text == switchName && item.SubItems[2].Text == ipText)
                        {
                            existingItem = item;
                            break;
                        }
                    }

                    if (existingItem != null)
                    {
                        existingItem.SubItems[1].Text = switchName;
                        existingItem.SubItems[2].Text = ipText;
                    }
                    else
                    {
                        ListViewItem newItem = new ListViewItem("");
                        newItem.SubItems.Add(switchName);
                        newItem.SubItems.Add(ipText);
                        ipListView.Items.Add(newItem);
                        ipListView.Invalidate();
                    }

                    ipTextField.Clear();
                    switchNameTB.Clear();
                    SaveData();
                }
                else
                {
                    UpdateLogger("Invalid IP address format.");
                }
            }
        }

        private void DeleteIpButton_Click(object sender, EventArgs e)
        {
            if (ipListView.SelectedItems.Count > 0)
            {
                var selectedItem = ipListView.SelectedItems[0];
                ipListView.Items.Remove(selectedItem);
                SaveData();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (macroListView.SelectedItems.Count > 0)
            {
                macroListView.Items.Remove(macroListView.SelectedItems[0]);
                SaveData();
            }
            else
            {
                UpdateLogger("No macro selected");
            }
        }

        private CancellationTokenSource cancellationTokenSource;

        private async void PlaybButton_Click(object sender, EventArgs e)
        {
            bots.Clear();
            InitializeBots();
            if (textBox1.Text == "")
            {
                UpdateLogger("No macro entered");
                return;
            }
            if (loopCheckbox.Checked == true)
            {
                pictureBoxStop.Enabled = true;
                pictureBoxStop.BackColor = Color.Aqua;
                pictureBoxPlay.Enabled = false;
                UpdateLogger("Starting Macro Loop");

            }

            cancellationTokenSource = new CancellationTokenSource();

            string commands = textBox1.Text;
            Func<bool> loopFunc = () => loopCheckbox.Checked;

            foreach (var bot in bots)
                try
                {
                    bot.Connect();
                    await bot.ExecuteCommands(commands, loopFunc, cancellationTokenSource.Token);
                    bot.Disconnect();
                }
                catch (Exception ex)
                {

                    UpdateLogger(ex.Message);
                }
        }

        private void StopbButton_Click(object sender, EventArgs e)
        {
            pictureBoxStop.BackColor = Color.White;
            pictureBoxPlay.Enabled = true;
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
                UpdateLogger("Stopping Macro");

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBoxStop.Enabled = false;

        }

        public void UpdateLogger(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateLogger), text);
            }
            else
            {
                logsBox.Text += (text + Environment.NewLine);
            }
        }

        private void LivebButton_Click(object sender, EventArgs e)
        {
            live = !live;
            string msg;
            if (live)
            {
                msg = "Live mode on";
                pictureBoxLive.BackColor = Color.FromArgb(253, 53, 58);
            }
            else
            {
                msg = "Live mode off";
                pictureBoxLive.BackColor = Color.White;
            }
            UpdateLogger(msg);
        }

        private async Task SendLiveButtonPress(string button)
        {
            InitializeBots();
            foreach (var bot in bots)
            {
                try
                {
                    bot.Connect();
                    switch (button)
                    {
                        case "A":
                            await bot.PressAButton();
                            break;
                        case "B":
                            await bot.PressBButton();
                            break;
                        case "X":
                            await bot.PressXButton();
                            break;
                        case "Y":
                            await bot.PressYButton();
                            break;
                        case "L":
                            await bot.PressLButton();
                            break;
                        case "R":
                            await bot.PressRButton();
                            break;
                        case "ZL":
                            await bot.PressZLButton();
                            break;
                        case "ZR":
                            await bot.PressZRButton();
                            break;
                        case "Up":
                            await bot.PressDPadUp();
                            break;
                        case "Down":
                            await bot.PressDPadDown();
                            break;
                        case "Left":
                            await bot.PressDPadLeft();
                            break;
                        case "Right":
                            await bot.PressDPadRight();
                            break;
                        case "Plus":
                            await bot.PressPlusButton();
                            break;
                        case "Minus":
                            await bot.PressMinusButton();
                            break;
                        case "Home":
                            await bot.PressHomeButton();
                            break;
                        case "Capture":
                            await bot.PressCaptureButton();
                            break;
                        case "LStick":
                            await bot.PressLeftStickButton();
                            break;
                        case "RStick":
                            await bot.PressRightStickButton();
                            break;
                        default:
                            throw new ArgumentException("Invalid button string.");
                    }
                    bot.Disconnect();
                }
                catch (Exception ex)
                {
                    UpdateLogger(ex.Message);
                }
            }
        }

        private async void BotStartBButton_Click(object sender, EventArgs e)
        {
            pictureBoxPlay.Enabled = false;
            pictureBoxLive.Enabled = false;


        }
        private async void BotStopBButton_Click(object sender, EventArgs e)
        {
            pictureBoxPlay.Enabled = true;
            pictureBoxLive.Enabled = true;

        }

    }

}

