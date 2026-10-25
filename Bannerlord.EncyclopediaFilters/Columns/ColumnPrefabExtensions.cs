using Bannerlord.UIExtenderEx.Attributes;
using Bannerlord.UIExtenderEx.Prefabs2;
using System.Collections.Generic;

namespace Bannerlord.EncyclopediaFilters.Columns;

// Rows: [name][sorted value][extra columns]. Widths are bound so other encyclopedia lists keep the vanilla layout.
[PrefabExtension("EncyclopediaItemListItem", "descendant::ScrollingTextWidget[@Id='ListItemNameTextWidget']")]
public sealed class ListItemNameWidthExtension : PrefabExtensionSetAttributePatch
{
    public override List<Attribute> Attributes => new()
    {
        new Attribute("SuggestedWidth", "@ColumnNameWidth"),
    };
}

[PrefabExtension("EncyclopediaItemListItem", "descendant::TextWidget[@Id='ListComparedValueTextWidget']")]
public sealed class ListItemPrimaryWidthExtension : PrefabExtensionSetAttributePatch
{
    public override List<Attribute> Attributes => new()
    {
        new Attribute("WidthSizePolicy", "Fixed"),
        new Attribute("SuggestedWidth", "@PrimaryColumnWidth"),
    };
}

[PrefabExtension("EncyclopediaItemListItem", "descendant::TextWidget[@Id='ListComparedValueTextWidget']")]
public sealed class ListItemColumnsExtension : PrefabExtensionInsertPatch
{
    public override InsertType Type => InsertType.Append;

    [PrefabExtensionText(true)]
    public string GetContent() => @"
<DiscardedRoot>
  <TextWidget WidthSizePolicy=""Fixed"" HeightSizePolicy=""CoverChildren"" SuggestedWidth=""95"" VerticalAlignment=""Center"" Brush=""EncyclopediaList.Filter.Name.Text"" Text=""@ExtraColumn1Value"" IsVisible=""@IsExtraColumn1Visible"" />
  <TextWidget WidthSizePolicy=""Fixed"" HeightSizePolicy=""CoverChildren"" SuggestedWidth=""95"" VerticalAlignment=""Center"" Brush=""EncyclopediaList.Filter.Name.Text"" Text=""@ExtraColumn2Value"" IsVisible=""@IsExtraColumn2Visible"" />
  <TextWidget WidthSizePolicy=""Fixed"" HeightSizePolicy=""CoverChildren"" SuggestedWidth=""95"" VerticalAlignment=""Center"" Brush=""EncyclopediaList.Filter.Name.Text"" Text=""@ExtraColumn3Value"" IsVisible=""@IsExtraColumn3Visible"" />
  <TextWidget WidthSizePolicy=""Fixed"" HeightSizePolicy=""CoverChildren"" SuggestedWidth=""95"" VerticalAlignment=""Center"" Brush=""EncyclopediaList.Filter.Name.Text"" Text=""@ExtraColumn4Value"" IsVisible=""@IsExtraColumn4Visible"" />
  <TextWidget WidthSizePolicy=""Fixed"" HeightSizePolicy=""CoverChildren"" SuggestedWidth=""95"" VerticalAlignment=""Center"" Brush=""EncyclopediaList.Filter.Name.Text"" Text=""@ExtraColumn5Value"" IsVisible=""@IsExtraColumn5Visible"" />
  <TextWidget WidthSizePolicy=""Fixed"" HeightSizePolicy=""CoverChildren"" SuggestedWidth=""95"" VerticalAlignment=""Center"" Brush=""EncyclopediaList.Filter.Name.Text"" Text=""@ExtraColumn6Value"" IsVisible=""@IsExtraColumn6Visible"" />
  <TextWidget WidthSizePolicy=""Fixed"" HeightSizePolicy=""CoverChildren"" SuggestedWidth=""95"" VerticalAlignment=""Center"" Brush=""EncyclopediaList.Filter.Name.Text"" Text=""@ExtraColumn7Value"" IsVisible=""@IsExtraColumn7Visible"" />
</DiscardedRoot>";
}

// Header: the same layout inside the sort controller's title bar. Extra headers sort the list when clicked.
[PrefabExtension("EncyclopediaItemList", "descendant::TextWidget[@Text='@NameLabel']")]
public sealed class HeaderNameWidthExtension : PrefabExtensionSetAttributePatch
{
    public override List<Attribute> Attributes => new()
    {
        new Attribute("SuggestedWidth", "@ColumnNameHeaderWidth"),
    };
}

[PrefabExtension("EncyclopediaItemList", "descendant::SortButtonWidget[@Command.Click='ExecuteSwitchSortOrder']")]
public sealed class HeaderPrimaryWidthExtension : PrefabExtensionSetAttributePatch
{
    public override List<Attribute> Attributes => new()
    {
        new Attribute("SuggestedWidth", "@PrimaryColumnHeaderWidth"),
    };
}

[PrefabExtension("EncyclopediaItemList", "descendant::SortButtonWidget[@Command.Click='ExecuteSwitchSortOrder']")]
public sealed class HeaderColumnsExtension : PrefabExtensionInsertPatch
{
    public override InsertType Type => InsertType.Append;

    [PrefabExtensionText(true)]
    public string GetContent() => @"
<DiscardedRoot>
  <SortButtonWidget DoNotPassEventsToChildren=""true"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""StretchToParent"" SuggestedWidth=""95"" MarginLeft=""@FirstExtraHeaderMargin"" Command.Click=""ExecuteSortByExtraColumn1"" IsSelected=""false"" SortState=""@ExtraColumn1SortState"" SortVisualWidget=""ExtraColumn1Label\ExtraColumn1Arrow"" UpdateChildrenStates=""true"" IsVisible=""@IsExtraColumn1Visible"">
    <Children>
      <TextWidget Id=""ExtraColumn1Label"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" VerticalAlignment=""Top"" MarginTop=""6"" Brush=""Encyclopedia.ItemList.SortedTitle"" Brush.FontSize=""18"" Text=""@ExtraColumn1Header"">
        <Children>
          <BrushWidget Id=""ExtraColumn1Arrow"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""14"" SuggestedHeight=""14"" MarginRight=""-14"" HorizontalAlignment=""Right"" VerticalAlignment=""Center"" Brush=""Encyclopedia.SortArrow"" />
        </Children>
      </TextWidget>
    </Children>
  </SortButtonWidget>
  <SortButtonWidget DoNotPassEventsToChildren=""true"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""StretchToParent"" SuggestedWidth=""95"" Command.Click=""ExecuteSortByExtraColumn2"" IsSelected=""false"" SortState=""@ExtraColumn2SortState"" SortVisualWidget=""ExtraColumn2Label\ExtraColumn2Arrow"" UpdateChildrenStates=""true"" IsVisible=""@IsExtraColumn2Visible"">
    <Children>
      <TextWidget Id=""ExtraColumn2Label"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" VerticalAlignment=""Top"" MarginTop=""6"" Brush=""Encyclopedia.ItemList.SortedTitle"" Brush.FontSize=""18"" Text=""@ExtraColumn2Header"">
        <Children>
          <BrushWidget Id=""ExtraColumn2Arrow"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""14"" SuggestedHeight=""14"" MarginRight=""-14"" HorizontalAlignment=""Right"" VerticalAlignment=""Center"" Brush=""Encyclopedia.SortArrow"" />
        </Children>
      </TextWidget>
    </Children>
  </SortButtonWidget>
  <SortButtonWidget DoNotPassEventsToChildren=""true"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""StretchToParent"" SuggestedWidth=""95"" Command.Click=""ExecuteSortByExtraColumn3"" IsSelected=""false"" SortState=""@ExtraColumn3SortState"" SortVisualWidget=""ExtraColumn3Label\ExtraColumn3Arrow"" UpdateChildrenStates=""true"" IsVisible=""@IsExtraColumn3Visible"">
    <Children>
      <TextWidget Id=""ExtraColumn3Label"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" VerticalAlignment=""Top"" MarginTop=""6"" Brush=""Encyclopedia.ItemList.SortedTitle"" Brush.FontSize=""18"" Text=""@ExtraColumn3Header"">
        <Children>
          <BrushWidget Id=""ExtraColumn3Arrow"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""14"" SuggestedHeight=""14"" MarginRight=""-14"" HorizontalAlignment=""Right"" VerticalAlignment=""Center"" Brush=""Encyclopedia.SortArrow"" />
        </Children>
      </TextWidget>
    </Children>
  </SortButtonWidget>
  <SortButtonWidget DoNotPassEventsToChildren=""true"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""StretchToParent"" SuggestedWidth=""95"" Command.Click=""ExecuteSortByExtraColumn4"" IsSelected=""false"" SortState=""@ExtraColumn4SortState"" SortVisualWidget=""ExtraColumn4Label\ExtraColumn4Arrow"" UpdateChildrenStates=""true"" IsVisible=""@IsExtraColumn4Visible"">
    <Children>
      <TextWidget Id=""ExtraColumn4Label"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" VerticalAlignment=""Top"" MarginTop=""6"" Brush=""Encyclopedia.ItemList.SortedTitle"" Brush.FontSize=""18"" Text=""@ExtraColumn4Header"">
        <Children>
          <BrushWidget Id=""ExtraColumn4Arrow"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""14"" SuggestedHeight=""14"" MarginRight=""-14"" HorizontalAlignment=""Right"" VerticalAlignment=""Center"" Brush=""Encyclopedia.SortArrow"" />
        </Children>
      </TextWidget>
    </Children>
  </SortButtonWidget>
  <SortButtonWidget DoNotPassEventsToChildren=""true"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""StretchToParent"" SuggestedWidth=""95"" Command.Click=""ExecuteSortByExtraColumn5"" IsSelected=""false"" SortState=""@ExtraColumn5SortState"" SortVisualWidget=""ExtraColumn5Label\ExtraColumn5Arrow"" UpdateChildrenStates=""true"" IsVisible=""@IsExtraColumn5Visible"">
    <Children>
      <TextWidget Id=""ExtraColumn5Label"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" VerticalAlignment=""Top"" MarginTop=""6"" Brush=""Encyclopedia.ItemList.SortedTitle"" Brush.FontSize=""18"" Text=""@ExtraColumn5Header"">
        <Children>
          <BrushWidget Id=""ExtraColumn5Arrow"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""14"" SuggestedHeight=""14"" MarginRight=""-14"" HorizontalAlignment=""Right"" VerticalAlignment=""Center"" Brush=""Encyclopedia.SortArrow"" />
        </Children>
      </TextWidget>
    </Children>
  </SortButtonWidget>
  <SortButtonWidget DoNotPassEventsToChildren=""true"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""StretchToParent"" SuggestedWidth=""95"" Command.Click=""ExecuteSortByExtraColumn6"" IsSelected=""false"" SortState=""@ExtraColumn6SortState"" SortVisualWidget=""ExtraColumn6Label\ExtraColumn6Arrow"" UpdateChildrenStates=""true"" IsVisible=""@IsExtraColumn6Visible"">
    <Children>
      <TextWidget Id=""ExtraColumn6Label"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" VerticalAlignment=""Top"" MarginTop=""6"" Brush=""Encyclopedia.ItemList.SortedTitle"" Brush.FontSize=""18"" Text=""@ExtraColumn6Header"">
        <Children>
          <BrushWidget Id=""ExtraColumn6Arrow"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""14"" SuggestedHeight=""14"" MarginRight=""-14"" HorizontalAlignment=""Right"" VerticalAlignment=""Center"" Brush=""Encyclopedia.SortArrow"" />
        </Children>
      </TextWidget>
    </Children>
  </SortButtonWidget>
  <SortButtonWidget DoNotPassEventsToChildren=""true"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""StretchToParent"" SuggestedWidth=""95"" Command.Click=""ExecuteSortByExtraColumn7"" IsSelected=""false"" SortState=""@ExtraColumn7SortState"" SortVisualWidget=""ExtraColumn7Label\ExtraColumn7Arrow"" UpdateChildrenStates=""true"" IsVisible=""@IsExtraColumn7Visible"">
    <Children>
      <TextWidget Id=""ExtraColumn7Label"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" VerticalAlignment=""Top"" MarginTop=""6"" Brush=""Encyclopedia.ItemList.SortedTitle"" Brush.FontSize=""18"" Text=""@ExtraColumn7Header"">
        <Children>
          <BrushWidget Id=""ExtraColumn7Arrow"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""14"" SuggestedHeight=""14"" MarginRight=""-14"" HorizontalAlignment=""Right"" VerticalAlignment=""Center"" Brush=""Encyclopedia.SortArrow"" />
        </Children>
      </TextWidget>
    </Children>
  </SortButtonWidget>
</DiscardedRoot>";
}

// The vertical divider sits in front of the first skill column (vanilla position otherwise).
[PrefabExtension("EncyclopediaItemList", @"descendant::Widget[@Sprite='Encyclopedia\divider_vertical']")]
public sealed class DividerPositionExtension : PrefabExtensionSetAttributePatch
{
    public override List<Attribute> Attributes => new()
    {
        new Attribute("MarginLeft", "@DividerMarginLeft"),
        new Attribute("IsVisible", "@IsDividerVisible"),
    };
}

// The 'Sort By' dropdown moves up next to the path bar so the title row has room for the columns.
// Gauntlet only hit-tests children inside their parent's area, so a negative margin would leave it unclickable.
// Instead the original is hidden and a copy lives in a container the size of the page, raised by 81.
[PrefabExtension("EncyclopediaItemList", "descendant::ListPanel[@Id='EncyclopediaListSortController' and @HorizontalAlignment='Right']")]
public sealed class SortDropdownHideExtension : PrefabExtensionSetAttributePatch
{
    public override List<Attribute> Attributes => new()
    {
        new Attribute("IsVisible", "false"),
    };
}

[PrefabExtension("EncyclopediaItemList", "descendant::Widget[@SuggestedHeight='!Encyclopedia.Height']")]
public sealed class SortDropdownRaisedExtension : PrefabExtensionInsertPatch
{
    public override InsertType Type => InsertType.Append;

    // Centered like the page: a top margin of 155 puts the page at (parent - height) / 2 + 77.5,
    // so -7 puts this container 81 higher.
    [PrefabExtensionText]
    public string GetContent() => @"
<Widget DoNotAcceptEvents=""true"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""!Encyclopedia.Width"" SuggestedHeight=""!Encyclopedia.Height"" HorizontalAlignment=""Center"" VerticalAlignment=""Center"" MarginTop=""-7"">
  <Children>
    <ListPanel Id=""EncyclopediaListSortController"" DataSource=""{SortController}"" WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" HorizontalAlignment=""Right"" MarginTop=""10"" MarginRight=""0"" StackLayout.LayoutMethod=""HorizontalRightToLeft"">
      <Children>
        <Standard.DropdownWithHorizontalControl Id=""EncyclopediaSortButton"" Parameter.IsFlatDesign=""true"" Parameter.ShowNextAndPrevious=""false"" Parameter.IsCustomWidth=""true"" Parameter.CustomWidth=""160"" Parameter.SelectorDataSource=""{SortSelection}"" Parameter.OnDropdownClick=""ExecuteOnDropdownActivated"" UpdateChildrenStates=""true"" ExtendCursorAreaRight=""-20"" GamepadNavigationIndex=""0"">
          <Children>
            <BrushWidget Id=""TypeSortVisualWidget"" DoNotAcceptEvents=""true"" WidthSizePolicy=""Fixed"" HeightSizePolicy=""Fixed"" SuggestedWidth=""20"" SuggestedHeight=""20"" HorizontalAlignment=""Right"" PositionXOffset=""-20"" VerticalAlignment=""Center"" Brush=""Encyclopedia.Dropdown.Arrow"" />
          </Children>
        </Standard.DropdownWithHorizontalControl>
        <TextWidget WidthSizePolicy=""CoverChildren"" HeightSizePolicy=""CoverChildren"" VerticalAlignment=""Center"" MarginTop=""5"" MarginRight=""10"" Brush=""Standard.DropdownItem.Text"" Text=""@SortByLabel"" />
      </Children>
    </ListPanel>
  </Children>
</Widget>";
}

[PrefabExtension("EncyclopediaItemList", "descendant::TutorialHighlightItemBrushWidget[@IsHighlightEnabled='@IsHighlightEnabled']")]
public sealed class SortDropdownHighlightPositionExtension : PrefabExtensionSetAttributePatch
{
    public override List<Attribute> Attributes => new()
    {
        new Attribute("PositionYOffset", "-70"),
    };
}
