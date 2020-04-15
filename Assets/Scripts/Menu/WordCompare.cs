using System.Collections.Generic;
using System;
using System.Collections;

class WordCompare : IEqualityComparer<Word>
{
    //比较复杂的数据类型，比如说比较两个单词对象，需要使用IEqualityComparer
    public bool Equals(Word x, Word y)
    {
        //判断两个单词的 spell 还有 ID 相同，说明是同一个单词
        if (x.WordId == y.WordId && x.Spell == y.Spell)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetHashCode(Word obj)
    {
        if(obj == null)
        {
            return 0;
        }
        else
        {
            return obj.ToString().GetHashCode();
        }
    }
}