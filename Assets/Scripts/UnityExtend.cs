using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
public static class UnityExtend
{
    public static Transform FindRecursively(this Transform transform, string name)
    {
        Transform result = null;
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                result = child;
                break;
            }
            else
            {
                result = child.FindRecursively(name);
                if (result != null)
                {
                    break;
                }
            }
        }

        return result;
    }

    public static T TryAddComponent<T>(this GameObject obj) where T : Component
    {
        T ret = obj.GetComponent<T>();
        if (ret == null)
        {
            ret = obj.AddComponent<T>();
        }

        return ret;
    }

    public static T TryAddComponent<T>(this RawImage rawImage) where T : Component
    {
        T ret = rawImage.GetComponent<T>();
        if (ret == null)
        {
            ret = rawImage.gameObject.AddComponent<T>();
        }

        return ret;
    }

    public static void SetActiveEx(this Transform trans,bool state)
    {
        if (trans.gameObject.activeSelf != state)
        {
            trans.gameObject.SetActive(state);
        }
    }

    public static void SetActiveEx(this GameObject obj, bool state)
    {
        if (obj.activeSelf != state)
        {
            obj.SetActive(state);
        }
    }

    public static void SetActiveEx(this Image img, bool state)
    {
        if (img.enabled != state)
        {
            img.enabled = state;
        }
    }

    public static void SetActiveEx(this RawImage img, bool state)
    {
        if (img.enabled != state)
        {
            img.enabled = state;
        }
    }

    public static void SetActiveEx(this Text text, bool state)
    {
        if (text.enabled != state)
        {
            text.enabled = state;
        }
    }

    /// <summary>
    /// 销毁一个物体下的所有子物体,并不会触发UI的OnDestory
    /// </summary>
    public static void ClearAllChildren(this Transform transform)
    {
        if (transform == null)
        {
            return;
        }

        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>           
    ///  复制           
    ///  </summary>             
    ///  <param name="destination">目标</param>             
    ///  <param name="source">来源</param>             
    ///  <returns>成功复制的值个数</returns>            
    public static int CopyObjectToAnother(object destination, object source)
    {
        if (destination == null || source == null)
        {
            return 0;
        }

        return CopyObjectToAnother(destination, source, source.GetType());
    }


    /// <summary>            
    /// 复制          
    /// </summary>            
    /// <param name="destination">目标</param>          
    /// <param name="source">来源</param>           
    /// <param name="type">复制的属性字段模板</param>          
    /// <returns>成功复制的值个数</returns>           
    public static int CopyObjectToAnother(object destination, object source, Type type)
    {
        return CopyObjectToAnother(destination, source, type, null);
    }

    /// <summary>           
    /// 复制          
    /// </summary>      
    /// <param name="destination">目标</param>        
    /// <param name="source">来源</param>           
    /// <param name="type">复制的属性字段模板</param>           
    /// <param name="excludeName">排除下列名称的属性不要复制</param>             
    /// <returns>成功复制的值个数</returns>            
    public static int CopyObjectToAnother(object destination, object source, Type type, IEnumerable<string> excludeName)
    {
        if (destination == null || source == null)
        {
            return 0;
        }

        if (excludeName == null)
        {
            excludeName = new List<string>();
        }

        int i = 0;
        Type desType = destination.GetType();
        foreach (FieldInfo mi in type.GetFields())
        {
            if (excludeName.Contains(mi.Name))
            {
                continue;
            }

            try
            {
                FieldInfo des = desType.GetField(mi.Name);
                if (des != null && des.FieldType == mi.FieldType)
                {
                    des.SetValue(destination, mi.GetValue(source));
                    i++;
                }
            }
            catch
            {
            }
        }

        foreach (PropertyInfo pi in type.GetProperties())
        {
            if (excludeName.Contains(pi.Name))
            {
                continue;
            }

            try
            {
                PropertyInfo des = desType.GetProperty(pi.Name);
                if (des != null && des.PropertyType == pi.PropertyType && des.CanWrite && pi.CanRead)
                {
                    des.SetValue(destination, pi.GetValue(source, null), null);
                    i++;
                }
            }
            catch
            {
                //throw ex;                  
            }
        }

        return i;
    }

    /// <summary>
    /// 让图片保持源Sprite的长宽比防止变形(当同一个Image组件需要反复调用这个方法时候,需要在每次调用之前将Image组件的尺寸设置为原始尺寸,否则图可能会越来越小)
    /// </summary>
    /// <param name="image"></param>
    /// <param name="baseHeight">以高度为基准适配,图片的宽度可能会超出容器</param>
    public static void KeepNativeRatio(this Image image, bool baseHeight = false)
    {
        Sprite sprite = image.sprite;
        if (sprite == null)
            return;

        float spriteWidth = sprite.rect.width;
        float spriteHeight = sprite.rect.height;

        float imageWidth = image.rectTransform.rect.width;
        float imageHeight = image.rectTransform.rect.height;

        if (spriteWidth < imageWidth && spriteHeight < imageHeight)
        {
            image.SetNativeSize();
            return;
        }

        if (baseHeight) //允许宽度超出容器
        {
            var ratio = spriteHeight / imageHeight;
            image.rectTransform.sizeDelta = new Vector2(spriteWidth / ratio, imageHeight);
        }
        else
        {
            // 计算匹配比例
            float aspectRatio = spriteWidth / spriteHeight;
            // 根据当前宽度和比例计算高度
            float targetHeight = imageWidth / aspectRatio;
            // 如果高度超出容器，则计算匹配宽度
            if (targetHeight > imageHeight)
            {
                float targetWidth = imageHeight * aspectRatio;
                image.rectTransform.sizeDelta = new Vector2(targetWidth, imageHeight);
            }
            else
            {
                image.rectTransform.sizeDelta = new Vector2(imageWidth, targetHeight);
            }
        }
    }

    public static T DeepCopy<T>(T obj)
    {
        // 如果是值类型或者字符串，直接返回
        if (obj is ValueType || obj is string)
        {
            return obj;
        }

        // 如果是引用类型
        object result = Activator.CreateInstance(obj.GetType());
        FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo field in fields)
        {
            object fieldValue = field.GetValue(obj);
            if (fieldValue == null)
            {
                continue;
            }

            field.SetValue(result, DeepCopy(fieldValue));
        }

        return (T)result;
    }

}