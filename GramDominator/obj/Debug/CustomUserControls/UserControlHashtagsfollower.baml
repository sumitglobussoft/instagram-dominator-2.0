   M S B A M L   `   `   `  ����H  DGramDominator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:GramDominator.ClassesGramDominator.Classes  5      B    <GramDominator.CustomUserControls.UserControlHashtagsfollower   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      3app#clr-namespace:GramDominator.Classes     5      .��5	      �� .��5
   
   �   � DataContext  5      &    GramDominator.Classes.Validation  5      -   5        objform��5   !    � ColumnDefinitions 5      �� 5      $
��206* �6#   �� 5      $	��30* �6#   �� 5      $
��243* �6#   �� 5      $��6* �6#   �� 5      $
��115* �6#    � RowDefinitions 5      �� 5      $	��26* �6    �� 5      $
��183* �6    �� 5      $	��41* �6    .+�5      �� $��0��6   $��Follow By HashTag��6%   $��Center=�6>   $
��Bold"�6[   $��18(�6m   $��	15,0,97,0q�6{   $��2��6�   $��1��6�   �� 5      $��Input��6   $��5��6&   $��0,0,0,54q�6:   $��1��6L   .��5      �  5      �� 5      $
��160* �6+   �� 5       $
��169* �6+   �� 5!      $
��121* �6+   �� 5"      $��9* �6+   �� 5#      $
��129* �6+    5%      �� 5&      $	��41* �6(   �� 5'      $	��46* �6(   .+�5)      	� -   5        $)��#rdoBtn_HashTags_follower_SingleUser��5)   "    d� 	IsChecked$
 TrueY�6M   $��Single HashTag��6^   $��1��6w   $��1��6�   $��25��6�    	� 	GroupName$ followByUserName��6�   $
��Left=�6�   $��
28,12,0,10q�6  $	��141��6"  	� 5*      -   5        $+��%rdoBtn_HashTags_follower_MultipleUser��5*   "   $��Multiple HashTag��6O   $��1��6j   $��2��6w   $��25��6�   $ followByUserName��6�   $��Right=�6�   $��	0,12,0,10q�6�   $	��121��6  �� 5+      -   5        $��Username��5+      $��HashTag��6,   $��0��6>   $��25��6N   $��Right=�6Z   $��Center=�6v   $��0,6,0,11q�6�   $	��111��6�   �� 5,      -   5        $)��#txt_HashTags_follower_LoadUsersPath��5,       �� 
IsReadOnly .  $��1��6\   $��25��6l   $��0,12,0,6q�6x   $��Center=�6�   $��2��6�   $�� ��6�   $��1,1,1,2q�6�   �� 5-      -   5        $'��!btn_HashTags_follower_BrowseUsers��5-      $y�Hidden=�6F   $��Browse��6Z   $��4��6k   $��25��6{   $��0,12,0,6q�6�   
��� � ���5.      �� 5/       �� 
TargetType" ��6.   .R�50   "   ��  �� CornerRadius$ 5��6)   
���W�.�$��1,1,1,2q�6Q   
�������._�51   &   �� $��Center=�67   $��Center=�6T   52   #   53      55      56      57      �� 58      $��No. of User��6   $��5,128,81,29q�6*   $��26��6?   $��Center=�6K   $��1��6f   �� 59      -   5        $)��#txtMessage_hashtagfollower_NoOfuser��59       �� TextAlignment$ Center=�6A   $��130,128,0,29q�6X   	 �� TextWrapping$
	 Wrap=�6n   $��5��6�   $��26��6�   $��Center=�6�   $��1��6�   $��1,1,1,2q�6�   �� 5:      -   5        
 ��Name$ 
 btn_HashTagefollower_Clear��5:      
����  �$��Clear��6L   $��4��6\   $��25��6l   $��
0,134,8,24q�6�   $��1��6�   ��5;      �� 5<      " ��6&   .R�5=      �� $ 5��6!   
����  �$��1,1,1,2q�6E   
�������._�5>      �� $��Center=�6/   $��Center=�6L   5?      5@      5B      �� 5C      -	   5        $��chkNotSendRequest��5C   D   $(��"Don't Send Request To Private User��6   $��2��6]   $
��Left=�6m   $��	0,129,0,0q�6�   $��1��6�   $	��Top=�6�   $	��243��6�   $��22��6�   6�   5D      5E      5F      