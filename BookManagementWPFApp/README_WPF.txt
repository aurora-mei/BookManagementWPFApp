Vào mỗi window cần sử dụng thì thêm dòng này vào đầu file xaml:{
  xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes"
TextElement.Foreground="{DynamicResource MaterialDesignBody}"
TextElement.FontWeight="Regular"
TextElement.FontSize="13"
TextOptions.TextFormattingMode="Ideal"
TextOptions.TextRenderingMode="Auto"
Background="{DynamicResource MaterialDesignPaper}"
FontFamily="{DynamicResource MaterialDesignFont}"
       WindowStartupLocation="CenterScreen"
       mc:Ignorable="d"
       Style="{StaticResource MaterialDesignWindow}"
}

