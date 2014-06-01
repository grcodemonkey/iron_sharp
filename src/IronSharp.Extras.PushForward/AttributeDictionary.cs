using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace IronSharp.Extras.PushForward
{
    internal class AttributeDictionary
        : Dictionary<string, string>
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AttributeDictionary" /> class by using the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public AttributeDictionary(object model)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            if (model != null)
            {
                foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(model))
                {
                    Add(item.Name, Convert.ToString(item.GetValue(model)));
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AttributeDictionary" /> class.
        /// </summary>
        public AttributeDictionary()
            : this(null)
        {
        }

        #endregion

        #region Public Methods

        public void Add(KeyValuePair<string, string> item)
        {
            Add(item.Key, item.Value);
        }

        #endregion
    }
}