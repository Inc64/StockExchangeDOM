   M S B A M L   `   `   `  ����K  GStockExchangeDOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:StockExchangeDOMStockExchangeDOM  5   	   G(clr-namespace:StockExchangeDOM.ViewModelStockExchangeDOM.ViewModel  5   	   ]3clr-namespace:StockExchangeDOM.ViewModel.Converters%StockExchangeDOM.ViewModel.Converters  5	   	   &     StockExchangeDOM.View.MainWindow   5   	   \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:StockExchangeDOM     5   	   ?
viewmodels(clr-namespace:StockExchangeDOM.ViewModel     5   	   J
converters3clr-namespace:StockExchangeDOM.ViewModel.Converters     5	   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5
   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)    � 	Resources 5      A   ;StockExchangeDOM.ViewModel.Converters.OfferToColorConverter   offer2color��%   &	          5   
   6+   5       � DataContext 5      5   /StockExchangeDOM.ViewModel.MainWindow_ViewModel  5   
   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5      +  %Microsoft.Xaml.Behaviors.EventTrigger  5   
      	EventName$ Closing��6   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 6$   �� *OnWindowClosing+5      .��5      � {�6   �� *IsContentEnabled+ �� UpdateSourceTrigger$ PropertyChanged=� � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5"   
   �� 	 �� Orientation$	 Vertical=�6   $��0��5#      .+�5$      �� $	 
Horizontal=�6   $��3q�62   .+�5%      �� $��Ticker: ��6   �� 5&      $
 �� IsSynchronizedWithCurrentItem$

 TrueY�6   $	��100��5'      ��5(      �� *
TickerList+ �� SelectedItem 5)      �� *TickerSelected+ �� Mode$ TwoWay=�$ PropertyChanged=�5+      5,      �� 5/   
   $
��Auto=�6   $��1��50      .��51      �� ��6   �� *TickersDepth+��52      �� 53      .R�54      �� $	 
Horizontal=�6(   ��55   (   ��  �� Path$ Offer� �� 	Converter# [  .+�56      �� $��Price: ��6(   �� 57      ��6(   �� $ Price��� 58      $��   Quantity: ��6(   �� 59      ��6(   �� $ Quantity�5:      5;      5=      5>      5@      5A      