using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UniTaskExtension
{
    public static async void WrapErrors(this UniTask self) { await self; }
    public static async void WrapErrors<T>(this UniTask<T> self) { await self; }
    public static async void WrapErrors(this Task self) { await self; }
    public static async void WrapErrors<T>(this Task<T> self) { await self; }

    public static bool WasThrown(this UniTask task, Type exceptionType)
    {
        try
        {
            task.GetAwaiter().GetResult();
            return false;
        }
        catch (Exception ex)
        {
            return ex.GetType().IsAssignableFrom(exceptionType);
        }
    }

    public static bool WasThrown(this UniTask task, Type exceptionType, out Exception exception)
    {
        try
        {
            task.GetAwaiter().GetResult();
            exception = null;
            return false;
        }
        catch (Exception ex)
        {
            exception = ex;
            return ex.GetType().IsAssignableFrom(exceptionType);
        }
    }

    public static bool WasThrown<T>(this UniTask<T> task, Type exceptionType)
    {
        try
        {
            var taskResult = task.GetAwaiter().GetResult();
            return false;
        }
        catch (Exception ex)
        {
            return ex.GetType().IsAssignableFrom(exceptionType);
        }
    }

    public static bool WasThrown<T>(this UniTask<T> task, Type exceptionType, out Exception exception)
    {
        try
        {
            var taskResult = task.GetAwaiter().GetResult();
            exception = null;
            return false;
        }
        catch (Exception ex)
        {
            exception = ex;
            return ex.GetType().IsAssignableFrom(exceptionType);
        }
    }
}

public static class CoreExtensions
{
    /// <summary>
    /// Gets the final exception from a raw exception.
    /// </summary>
    public static Exception FinalException(this Exception exception)
    {
        var ex = exception;
        while (true)
        {
            if (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            else
            {
                break;
            }
        }

        return ex;
    }

    /// <summary>
    /// Change Alpha of given graphic component
    /// </summary>
    public static void ChangeAlpha(this Graphic g, float newAlpha)
    {
        var color = g.color;
        color.a = newAlpha;
        g.color = color;
    }

    public static byte[] ReverseLittleEndian(this byte[] self)
    {
        if (!System.BitConverter.IsLittleEndian)
            Array.Reverse(self);

        return self;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void SetValue(this MemberInfo self, object target, object value)
    {
        if (self is FieldInfo)
            ((FieldInfo)self).SetValue(target, value);
        else if (self is PropertyInfo)
            ((PropertyInfo)self).SetValue(target, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static object GetValue(this MemberInfo self, object target)
    {
        if (self is FieldInfo)
            return ((FieldInfo)self).GetValue(target);
        else if (self is PropertyInfo)
            return ((PropertyInfo)self).GetValue(target);

        return null;
    }

    /// <summary>
    /// Wait for second.
    /// </summary>
    public static Coroutine WaitFor<T>(this T self, float time, Action completed) where T : MonoBehaviour
    {
        return self.StartCoroutine(WaitForSecond(time, completed));
    }

    private static IEnumerator WaitForSecond(float time, Action completed)
    {
        if (time <= 0)
            yield return null;
        else
            yield return new WaitForSeconds(time);

        completed?.Invoke();
    }

    /// <summary>
    /// 
    /// </summary>
    public static HashSet<T> AddRange<T>(this HashSet<T> self, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            self.Add(item);
        }

        return self;
    }

    /// <summary>
    /// 
    /// </summary>
    public static void AddEventTrigger(this EventTrigger eventTrigger, EventTriggerType triggerType, UnityAction<BaseEventData> callback)
    {
        var pointer = new EventTrigger.Entry();
        pointer.eventID = triggerType;
        pointer.callback.AddListener(callback);
        eventTrigger.triggers.Add(pointer);
    }
}