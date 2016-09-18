using System.ComponentModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Demo.Presentation.Behaviours
{
    /// <summary>
    /// A behaviour that uses checks the properties of items in the DataGrid for <see cref="DisplayNameAttribute"/>
    /// and uses the <see cref="DisplayNameAttribute.DisplayName"/> for column headers.
    /// </summary>
    /// <remarks>
    /// See :
    /// http://www.codeproject.com/Articles/389764/A-Smart-Behavior-for-DataGrid-AutoGenerateColumn
    /// </remarks>
    public class ColumnHeaderDisplayBehaviour : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.AutoGeneratingColumn +=
                AutoGeneratingColumn;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.AutoGeneratingColumn -=
                AutoGeneratingColumn;
        }

        private void AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private string GetPropertyDisplayName(object descriptor)
        {
            PropertyDescriptor propertyDescripter = descriptor as PropertyDescriptor;
            if(propertyDescripter != null)
            {
                DisplayNameAttribute attr = propertyDescripter.Attributes[typeof(DisplayNameAttribute)] 
                    as DisplayNameAttribute;

                if(attr != null && attr != DisplayNameAttribute.Default)
                {
                    return attr.DisplayName;
                }
            }
            else
            {
                PropertyInfo property = descriptor as PropertyInfo;
                if(property != null)
                {
                    object [] attrs = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);

                    foreach(var attr in attrs)
                    {
                        DisplayNameAttribute attribute = attr as DisplayNameAttribute;
                        if ((attribute != null) && (attribute != DisplayNameAttribute.Default))
                        {
                            return attribute.DisplayName;
                        }
                    }
                }
            }
            return null;
        }
    }
}
