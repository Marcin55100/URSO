   M S B A M L   `   `   `  ����C  ?URSO_LED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:URSO_LED.ViewsURSO_LED.Views  5          URSO_LED.Views.OutputView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .localclr-namespace:URSO_LED.Views     5      .��5	      �   � ColumnDefinitions  5
   
   �� 5      $��1* �6   �� 5      $��1* �6   .+�5   
   � $��0��6    � RowDefinitions 5      �� 5      $��* �6    �� 5      $��* �6    .+�5      � $��0��6   .+�5      ��  �� Orientation$ Vertical=�6   .+�5      �� $
��Bold"�6   $��Nowa konfiguracja:��6.   �� 5      $ 
Horizontal=�6!   .+�5      �� $��Wpisz nazwę:��6    �� 5      -   5        $��configNameBox��5   "   $��50��6S    �� TextWrapping$
 Wrap=�6a   ��67   �� *
ConfigName+6a   5      �� 5      $ 
Horizontal=�6!   .+�5      �� -   5        $��addConfigButton��5   !   $��Utwórz��6`    � DataContext 68   �� *SelectedNetwork+��6q   �� *AddConfiguration+��5      �� 5   "    �� 
TargetType" ��6(   .�5    &   ��  �� Property � {� �� Value . 6B    �� Triggers 5!   &   �� 5"   *   	 �� Binding	 66   �� 
 �� Source$
 SelectedNetwork�� �� Path$
 Font� �� Value Bold6l   .a�5#   .   ��  � {� .  6J   5$   +   5&   #   5(      �� 5)      -   5        $��saveConfigButton��5)   !   $��Zapisz��69   ��6J   �� *SaveConfiguration+��5*      �� 5+   "   " ��6(   .�5,   &   ��  � {� . 6B    5-   &   �� 5.   *   	 66   �� *Text+ �� ElementName$ configNameBox��  6j   .a�5/   .   ��  � {� .  6J   50   +   52   #   5:      5<      5?      5@      � 5B      $��1��6   .+�5C      �� $ Vertical=�6   .+�5D      �� $
��Bold"�6   $��Istniejąca konfiguracja:��6.   �� 5E      -   5         ��Name$ fileBox��5E      $��50��5G   F    �� SelectedValue 5F      �� *SelectedConfig+��5G      �� $ 
ConfigList�6F   �� 5H      $ 
Horizontal=�6!   .+�5I      �� -   5        $��removeConfigButton��5I   !   $��Usuń��6c   ��6;   �� *RemoveConfiguration+6c   5J      5K      5L      5N      � 5S   
   $��1��6    5T      �� 5U      $��2* �6    �� 5V      $��* �6    .+�5X      �� -   5        $��listView��5X   $   $��0��6    �� SelectedItem 5Y      �� $ SelectedSegment���5Z      �� $ SegmentList� �� View 5[      �� 5\      .P�5]      �� $��Id��6)    �� DisplayMemberBinding 65   �� *Id+�� 5^      $��Nazwa��6)    68   �� *Name+�� 5_      $	��Typ��6)    66   �� *Type+5`      5b      � 5d      $��1��6   .+�5e      �� y�6   �� *
Segments+.+�5f      �� $
��Bold"�6   $��	Segmenty:��6.   �� 5g      $ 
Horizontal=�6   .+�5h      �� $��Nazwa:��6   �� 5i      -   5        $��
ledNameBox��5i      $��50��6M   $
 Wrap=�6[   ��60   �� *SegmentName+6[   5j      �� 5k      $ 
Horizontal=�6   .+�5l      �� $
��Typ:��6   �� 5m      -   5        $��TypeBox��5m   #   ��62   �� $ TypeList� 5n      �� $ SegmentType�5p      �� 5q      $ 
Horizontal=�6   .+�5r      �� -	   5        $��addSegmentButton��5r   !   $��Dodaj��69   ��6I   �� *
AddSegment+5s      �� 5t      -
   5        $��removeButton��5t   !   $��Usuń��6W   ��65   �� *RemoveSegment+6W   5u      5w      5z      5�      5�      5�      