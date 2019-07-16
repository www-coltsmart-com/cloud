using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColtSmart.Entity.Entities;

namespace Coltsmart.Portal.Models
{
    /// <summary>
    /// 产品Model类
    /// </summary>
    public class GoodsModel
    {
        public int id { get; set; }

        public string name { get; set; }

        public string picture { get; set; }

        public string info { get; set; }

        public string description { get; set; }

        public int displayOrder { get; set; }

        public int status { get; set; }

        public IEnumerable<GoodsAttrModel> attrs { get; set; }

        public IEnumerable<GoodsAttachModel> downloads { get; set; }
    }

    public class GoodsAttrModel
    {
        public string name { get; set; }

        public string value { get; set; }

        public int displayOrder { get; set; }
    }

    public class GoodsAttachModel
    {

        public string name { get; set; }

        public string path { get; set; }

        public string ext { get; set; }

        public int size { get; set; }

        public string url { get; set; }
    }

    public static class GoodsExtendtions
    {
        public static Goods ToEntity(this GoodsModel value)
        {
            return new Goods()
            {
                id = value.id,
                Name = value.name,
                Picture = value.picture,
                Info = value.info,
                Description = value.description,
                DisplayOrder = value.displayOrder,
                Status = value.status,
            };
        }

        public static GoodsModel ToModel(this Goods item)
        {
            return new GoodsModel()
            {
                id = item.id,
                name = item.Name,
                picture = item.Picture,
                info = item.Info,
                description = item.Description,
                displayOrder = item.DisplayOrder,
                status = item.Status
            };
        }

        public static IEnumerable<GoodsAttr> ToEntity(this IEnumerable<GoodsAttrModel> value)
        {
            List<GoodsAttr> results = new List<GoodsAttr>();
            foreach (var item in value)
            {
                results.Add(new GoodsAttr()
                {
                    Name = item.name,
                    Value = item.value,
                    DisplayOrder = item.displayOrder
                });
            }
            return results;
        }

        public static IEnumerable<GoodsAttrModel> ToModel(this IEnumerable<GoodsAttr> items)
        {
            List<GoodsAttrModel> results = new List<GoodsAttrModel>();
            foreach (var item in items)
            {
                results.Add(new GoodsAttrModel()
                {
                    name = item.Name,
                    value = item.Value,
                    displayOrder = item.DisplayOrder
                });
            }
            return results;
        }

        public static IEnumerable<GoodsAttach> ToEntity(this IEnumerable<GoodsAttachModel> value)
        {
            List<GoodsAttach> results = new List<GoodsAttach>();
            foreach (var item in value)
            {
                results.Add(new GoodsAttach()
                {
                    Name = item.name,
                    Ext = item.ext,
                    Size = item.size,
                    Path = item.url,
                });
            }
            return results;
        }

        public static IEnumerable<GoodsAttachModel> ToModel(this IEnumerable<GoodsAttach> items)
        {
            List<GoodsAttachModel> results = new List<GoodsAttachModel>();
            foreach (var item in items)
            {
                results.Add(new GoodsAttachModel()
                {
                    name = item.Name,
                    ext = item.Ext,
                    size = item.Size,
                    path = item.Path,
                    url = item.Path
                });
            }
            return results;
        }
    }
}
