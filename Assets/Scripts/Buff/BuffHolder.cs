using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuffHolder
{
    private Character character;

    private LinkedList<Buff> _buffs = new LinkedList<Buff>();

    public event UnityAction<Buff> BuffAdded;

    public event UnityAction<Buff> BuffRemoved;

    public BuffHolder(Character character)
    {
        this.character = character;
    }

    public bool AddBuff(BuffInfo buffInfo, int level = 1)
    {
        if (!CanAddBuff(buffInfo)) return false;
        var find = FindBuff(buffInfo);
        if (find == null)
        {
            //Ìí¼ÓÐÂBuff
            Buff buff = new Buff(buffInfo, buffInfo.Duration, level);
            _buffs.AddLast(buff);
            BuffAdded?.Invoke(buff);
            buff.Data.OnAdded(character);
        }
        else
        {
            find.AddLevel(level);
            CheckBuffConverter(find);
        }
        //Debug.Log($"Add {buffInfo.Name}");
        return true;
    }

    public void Tick()
    {
        var node = _buffs.First;
        while (node != null)
        {
            var nextNode = node.Next;
            node.Value.Tick(character);
            if (node.Value.DurationTimer <= 0)
            {
                RemoveBuffNode(node);
            }
            node = nextNode;
        }
    }

    private bool CanAddBuff(BuffInfo data)
    {
        foreach (var converter in data.converters)
        {
            if (HasBuff(converter.convertTarget))
                return false;
        }
        return true;
    }

    private void CheckBuffConverter(Buff buff)
    {
        bool removeBuff = false;
        foreach (var converter in buff.Data.converters)
        {
            if (buff.Level >= converter.convertLevel)
            {
                AddBuff(converter.convertTarget);
                removeBuff = true;
            }
        }
        if (removeBuff)
        {
            _buffs.Remove(buff);
            OnBuffRemoved(buff);
        }
    }

    private void RemoveBuffNode(LinkedListNode<Buff> node)
    {
        _buffs.Remove(node);
        OnBuffRemoved(node.Value);
        //BuffRemoved?.Invoke(node.Value);
    }

    //public void OnTakeDamage(ref float damage)
    //{
    //    foreach (var buff in _buffs)
    //    {
    //        buff.Data.OnTakeDamage(ref damage);
    //    }
    //}

    //public bool RemoveBuff(BuffInfo buffInfo)
    //{

    //}

    public bool HasBuff(int buffId)
    {
        return FindBuff(buffId) != null;
    }

    public bool HasBuff(BuffInfo info) =>
        FindBuff(info) != null;

    public bool RemoveBuff(int id)
    {
        OnBuffRemoved(FindBuff(id));
        return _buffs.Remove(FindBuff(id));
    }

    private Buff FindBuff(BuffInfo info)
    {
        foreach (var buff in _buffs)
        {
            if (buff.Data == info)
                return buff;
        }
        return null;
    }

    private Buff FindBuff(int id)
    {
        foreach (var buff in _buffs)
        {
            if (buff.Data.Id == id)
                return buff;
        }
        return null;
    }

    private void OnBuffRemoved(Buff buff)
    {
        BuffRemoved?.Invoke(buff);
        buff.Data.OnRemoved(character);
    }
}
