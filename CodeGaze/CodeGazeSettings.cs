using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace CodeGaze
{
    internal class CodeGazeSettings
    {
        private readonly Dictionary<string, object> settings = new Dictionary<string, object>();
        private readonly string path;

        public event System.EventHandler SettingsChanged;

        public int Marker1IndentLevel
        {
            get => (int)this.settings[nameof(Marker1IndentLevel)];
            set
            {
                this.settings[nameof(Marker1IndentLevel)] = value;
                this.RaiseSettingsChanged();
            }
        }

        public string Marker1Text
        {
            get => (string)this.settings[nameof(Marker1Text)];
            set
            {
                this.settings[nameof(Marker1Text)] = value;
                this.RaiseSettingsChanged();
            }
        }

        public Color Marker1Color
        {
            get => (Color)this.settings[nameof(Marker1Color)];
            set
            {
                this.settings[nameof(Marker1Color)] = value;
                this.RaiseSettingsChanged();
            }
        }

        public int Marker2IndentLevel
        {
            get => (int)this.settings[nameof(Marker2IndentLevel)];
            set
            {
                this.settings[nameof(Marker2IndentLevel)] = value;
                this.RaiseSettingsChanged();
            }
        }

        public string Marker2Text
        {
            get => (string)this.settings[nameof(Marker2Text)];
            set
            {
                this.settings[nameof(Marker2Text)] = value;
                this.RaiseSettingsChanged();
            }
        }

        public Color Marker2Color
        {
            get => (Color)this.settings[nameof(Marker2Color)];
            set
            {
                this.settings[nameof(Marker2Color)] = value;
                this.RaiseSettingsChanged();
            }
        }

        public CodeGazeSettings(string configPath)
        {
            this.path = configPath;

            this.settings[nameof(Marker1IndentLevel)] = 6;
            this.settings[nameof(Marker1Text)] = "ಠ_ಠ";
            this.settings[nameof(Marker1Color)] = Color.FromArgb(255, 153, 35, 29);

            this.settings[nameof(Marker2IndentLevel)] = 9;
            this.settings[nameof(Marker2Text)] = "(╯°□°）╯︵ ┻━┻";
            this.settings[nameof(Marker2Color)] = Color.FromArgb(255, 153, 35, 29);
        }

        public void Write()
        {
            try
            {
                var dir = Path.GetDirectoryName(this.path);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                using (var writer = new StreamWriter(path, append: false))
                {
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}={1}", nameof(Marker1Text), this.Marker1Text));
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}={1}", nameof(Marker1IndentLevel), this.Marker1IndentLevel));
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}={1}", nameof(Marker1Color), this.Marker1Color.Serialize()));
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}={1}", nameof(Marker2Text), this.Marker2Text));
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}={1}", nameof(Marker2IndentLevel), this.Marker2IndentLevel));
                    writer.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0}={1}", nameof(Marker2Color), this.Marker2Color.Serialize()));
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(Application.Current.MainWindow, exc.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Read()
        {
            try
            {
                if (File.Exists(this.path))
                {
                    using (var reader = new StreamReader(this.path))
                    {
                        string l;
                        while ((l = reader.ReadLine()) != null)
                        {
                            var parts = l.Split(new char[] { '=' });
                            if (parts.Length == 2)
                            {
                                var key = parts[0].Trim();
                                var value = parts[1].Trim();

                                switch (key)
                                {
                                    case nameof(Marker1Text): this.Marker1Text = value; break;
                                    case nameof(Marker1IndentLevel): this.Marker1IndentLevel = int.Parse(value, CultureInfo.InvariantCulture); break;
                                    case nameof(Marker1Color): this.Marker1Color = value.DeserializeColor(); break;
                                    case nameof(Marker2Text): this.Marker2Text = value; break;
                                    case nameof(Marker2IndentLevel): this.Marker2IndentLevel = int.Parse(value, CultureInfo.InvariantCulture); break;
                                    case nameof(Marker2Color): this.Marker2Color = value.DeserializeColor(); break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(Application.Current.MainWindow, exc.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void RaiseSettingsChanged() => this.SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
}
