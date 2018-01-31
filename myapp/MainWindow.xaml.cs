﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using MaterialDesignThemes.Wpf;
using System.Reflection;

namespace myapp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Boolean is_max { set; get; }
        
        //private class cursor_types
        //{
        //    Boolean Hole { get; set; }
        //    Boolean Bomb_s { get; set; }
        //    Boolean Bomb_a{ get; set; }
        //    Boolean Draft { get; set; }
        //    Boolean Eraser { get; set; }
        //    Boolean Gp { get; set; }
        //    Boolean Gx { get; set; }
        //    Boolean Gm { get; set; }
        //    Boolean Move { get; set; }

        //}
        public Cursors_type cty { get; set; } 

        //public Boolean cursor_st { set; get; }
        public sealed class cursorhelper
        {
            private cursorhelper() { }
            public static Cursor frombytearray(byte[] array)
            {
                using (MemoryStream memory = new MemoryStream(array))
                {
                    return new Cursor(memory);
                }
            }

        }
        public Cursor cur { set; get; }
     
        public MainWindow()

        {
            cur = cursorhelper.frombytearray(Properties.Resources.normal_select_blue);
            this.Cursor = cur;
            InitializeComponent();
            is_max = false;
            //cursor_st = false;
            ut = new Utils() { Kind_type = "Check" };
            this.DataContext = ut;
            //鼠标标记初始化
            cty = new Cursors_type();
            //属性栏隐藏
            //this.property_panel.Visibility = Visibility.Hidden;
        }

        private void drag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void close_wd(object sender, MouseButtonEventArgs e)
        {
            //this.Close();
            Application.Current.Shutdown();
        }

        private void max_wd(object sender, MouseButtonEventArgs e)
        {
            if (!is_max)
            {
                this.WindowState = WindowState.Maximized;
                is_max = true;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                is_max = false;
            }
        }

        private void min_wd(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }

        //设置鼠标类型代理
        private bool Cur_rep(String name)
        {
            foreach (PropertyInfo p in cty.GetType().GetProperties())
            {
                if (p.Name.Equals(name))
                {
                    this.test_label.Text = name;
                    p.SetValue(cty, true, null);
                    return true;
                }
                else
                    p.SetValue(cty, false, null);
            }
            return false;
        }

        private void Set_cir_cursor(object sender, RoutedEventArgs e)
        {
            //if (cursor_st == false)
            //{
            cur = cursorhelper.frombytearray(Properties.Resources.circ);
            this.Cursor = cur;
            if (Cur_rep("Hole")) this.test_label.Text = "打孔";
            //change_cur_sta();
            //}
            //    else
            //    {
            //        change_cur_sta();
            //        init_cur();
            //    }
            //}
        }

        private void Set_pen_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.handwriting);
            this.Cursor = cur;
            if (Cur_rep("Draft")) this.test_label.Text = "备注";
        }

        private void Set_bomb_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.bomb);
            this.Cursor = cur;
            if (Cur_rep("Bomb_s")) this.test_label.Text = "连续装药";
        }

        private void Set_bomba_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.bomb_a);
            this.Cursor = cur;
            if (Cur_rep("Bomb_a")) this.test_label.Text = "空气炸药";
        }

        private void Set_era_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.eraser);
            this.Cursor = cur;
            if (Cur_rep("Eraser")) this.test_label.Text = "橡皮擦";
        }

        private void Set_crossp_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
            if (Cur_rep("Gp")) this.test_label.Text = "分组添加";
        }

        private void Set_crossm_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
            if (Cur_rep("Gx")) this.test_label.Text = "分组交叉";
        }

        private void Set_crossl_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
            if (Cur_rep("Gm")) this.test_label.Text = "分组移除";
        }

        private void Set_mouse_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.normal_select_blue);
            this.Cursor = cur;
            if (Cur_rep("Move")) this.test_label.Text = "移动";

        }

        private Utils ut;

        private void show_property(object sender, RoutedEventArgs e)
        {
            this.property_panel.Visibility = Visibility.Hidden;
        }

        private void ShowGridlines_OnChecked(object sender, RoutedEventArgs e)
        {
            DrawGraph((int)slidval.Value, (int)slidval.Value, ShapeCanvas);
            slidval.IsEnabled = true;
        }

        private void ShowGridlines_OnUnchecked(object sender, RoutedEventArgs e)
        {
            RemoveGraph(ShapeCanvas);
            slidval.IsEnabled = false;
        }

        private void SliderValue_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ShowGridlines.IsChecked ?? false)
            {
                //DrawGraph((int)SliderValue.Value, (int)SliderValue.Value, ShapeCanvas);
                DrawGraph((int)slidval.Value, (int)slidval.Value, ShapeCanvas);
            }
        }

        Brush _color1 = Brushes.Brown;
        Brush _color2 = Brushes.Cyan;
        private void DrawGraph(int yoffSet, int xoffSet, Canvas mainCanvas)
        {
            RemoveGraph(mainCanvas);
            Image lines = new Image();
            lines.SetValue(Panel.ZIndexProperty, -100);
            //Draw the grid
            DrawingVisual gridLinesVisual = new DrawingVisual();
            DrawingContext dct = gridLinesVisual.RenderOpen();
            Pen lightPen = new Pen(_color1, 0.5), darkPen = new Pen(_color2, 1);
            lightPen.Freeze();
            darkPen.Freeze();

            int yOffset = yoffSet,
                xOffset = xoffSet,
                rows = (int)(this.main_canvas.ActualHeight),
                columns = (int)(this.main_canvas.ActualWidth),
                alternate = yOffset == 5 ? yOffset : 1,
                j = 0;

            //Draw the horizontal lines
            Point x = new Point(0, 0.5);
            Point y = new Point(this.main_canvas.ActualWidth, 0.5);

            for (int i = 0; i <= rows; i++, j++)
            {
                dct.DrawLine(j % alternate == 0 ? lightPen : darkPen, x, y);
                x.Offset(0, yOffset);
                y.Offset(0, yOffset);
            }
            j = 0;
            //Draw the vertical lines

            x = new Point(0.5, 0);

            y = new Point(0.5, this.main_canvas.ActualHeight);

            for (int i = 0; i <= columns; i++, j++)
            {
                dct.DrawLine(j % alternate == 0 ? lightPen : darkPen, x, y);
                x.Offset(xOffset, 0);
                y.Offset(xOffset, 0);
            }
            dct.Close();
            this.test_label.Text = this.main_canvas.ActualHeight.ToString();
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)this.main_canvas.ActualWidth,
                (int)this.main_canvas.ActualHeight-50, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(gridLinesVisual);
            bmp.Freeze();
            lines.Source = bmp;

            mainCanvas.Children.Add(lines);
        }

        private void RemoveGraph(Canvas mainCanvas)
        {
            foreach (UIElement obj in mainCanvas.Children)
            {
                if (obj is Image)
                {
                    mainCanvas.Children.Remove(obj);
                    break;
                }
            }
        }

        private void cursor_event_handlers(object sender, MouseButtonEventArgs e)
        {
               
        }

        //public void init_cur()
        //{
        //    cur = cursorhelper.frombytearray(Properties.Resources.normal_select_blue);
        //    this.Cursor = cur;
        //}
        //public void change_cur_sta()
        //{
        //    cursor_st=cursor_st?false:true;
        //}
    }
}
