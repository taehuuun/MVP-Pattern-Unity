using System;
using System.Collections.Generic;
using UnityEngine;

public class MvpMethodBridge
{
    private readonly Dictionary<Enum, Delegate> _methods = new();
    
    private const string _METHOD_NOT_REGISTERED_MSG = "[{0}] 메서드 타입 {1}이(가) 등록되지 않음";
    private const string _METHOD_SIGNATURE_MISS_MATCHING_MSG = "[{0}] 메서드 타입 {1}에 호환되지 않는 시그니처";    
    public void AddMethod(Enum methodType, Delegate unityAction)
    {
        if (!_methods.TryAdd(methodType, unityAction))
        {
            Debug.LogError($"[{GetType().Name}] 메서드 타입{methodType}은(는) 이미 등록 됨");
        }
    }

    /// <summary>
    /// 등록된 메서드를 호출합니다.
    /// </summary>
    public void InvokeMethod(Enum methodType, params object[] parameters)
    {
        var method = GetMethod(methodType);

        if (method == null)
        {
            return;
        }
        
        method.DynamicInvoke(parameters);
    }

    /// <summary>
    /// 반환값이 필요한 메서드를 호출합니다.
    /// </summary>
    public TResult InvokeMethod<TResult>(Enum methodType, params object[] parameters)
    {
        var method = GetMethod(methodType);

        if (method == null)
        {
            return default;
        }
        
        return (TResult)method.DynamicInvoke(parameters);
    }

    public void RemoveMethod(Enum methodType)
    {
        if (!_methods.Remove(methodType))
        {
            Debug.LogError($"[{GetType().Name}] 메서드 타입 {methodType}이(가) 등록되지 않음");
        }
    }
    
    private Delegate GetMethod(Enum methodType)
    {
        if (_methods.TryGetValue(methodType, out var method))
        {
            if (method != null)
            {
                return method;
            }
    
            Debug.LogError(string.Format(_METHOD_SIGNATURE_MISS_MATCHING_MSG, GetType().Name, methodType));
        }
        else
        {
            Debug.LogError(string.Format(_METHOD_NOT_REGISTERED_MSG, GetType().Name, methodType));
        }
    
        return null;
    }
}
