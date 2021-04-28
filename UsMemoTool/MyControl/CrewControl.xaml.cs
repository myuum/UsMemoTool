using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UsMemoTool.Data.Crew;
using UsMemoTool.Utility;

namespace UsMemoTool
{
    /// <summary>
    /// CrewUserControl.xaml の相互作用ロジック
    /// </summary>
    public partial class CrewControl : UserControl
    {
        //イベント用デリゲート
        public delegate void IconMouseLeftButtonDown(object sender, EventArgs e);
        public event IconMouseLeftButtonDown iconClick;
        public event IconMouseLeftButtonDown IconClick 
        { 
            add =>iconClick += value;
            remove=>iconClick -= value;
        }
		private Crew crew = new Crew(PlayerColor.Black);
        public PlayerColor CrewColor
        {
            get => crew.Color;
            set
            {
                crew = new Crew(value);
                setCrewImage();
            }
		}
        private ImageSource crewImage;
        public ImageSource CrewImage 
        {
            get => crewImage;
            private set
            {
                crewImage = value;
                CrewIcon.Source = crewImage;
            }
        }
        private Brush aliveColor;
        public Brush AliveColor
        {
            get => aliveColor; 
            set
            {
                aliveColor = value;
                setAliveBtn();
            }
        }
        private Brush diedColor;
        public Brush DiedColor
        {
            get => diedColor;
            set
            {
                diedColor = value;
                setAliveBtn();
            }
        }
        private bool isAlive = true;
        public bool IsAlive 
        { 
            get => isAlive; 
            set 
            {
                isAlive = value;
                setAliveBtn();
            } 
        }
        private bool isUsed = false;
        public bool IsUsed 
        { 
            get=>isUsed; 
            set 
            {
                isUsed = value;
                setUsedBtn();
            }
        }
        List<MenuItem> menuItems = new List<MenuItem>();
        public CrewControl()
        {
            InitializeComponent();
            setUsedBtn();
            setAliveBtn();
            IsAliveBtn.Click += (sender, e) =>
            {
                IsAlive = !IsAlive;
            };
            UsedBtn.Click += (sender, e) =>
            {
                IsUsed = !IsUsed;
            };
            var menu = new ContextMenu();
            var colors = Enum.GetValues(typeof(PlayerColor));
            foreach (PlayerColor color in colors)
            {
                var item = new MenuItem();
                item.Header = color.ToAliasString();
                item.Click += (sender, e) =>
                {
                    CrewColor = color;
                };
                menuItems.Add(item);
                menu.Items.Add(item);
            }
            CrewIcon.ContextMenu = menu;
            CrewIcon.MouseLeftButtonDown += (sender, e) =>
            {
                if (iconClick == null) return;
                sender = crew;
                iconClick(sender, e);
            };
        }

		private void setUsedBtn()
        {
            UsedBtn.Content = isUsed ? "使用済" : "保持";
        }
        private void setAliveBtn()
        {
            IsAliveBtn.Background = isAlive ? AliveColor : DiedColor;
            IsAliveBtn.Content = isAlive ? "生存" : "死亡";
            setCrewImage();
        }
        private void setCrewImage()
        {
            CrewImage = isAlive ? crew.AriveImage : crew.DiedImage;
        }
		public override string ToString()
		{
            return crew.Color.ToString() + "_Crew";

        }
	}
}
