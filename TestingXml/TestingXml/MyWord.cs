using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingXml
{
    public class MyWord
    {
        private String _theWord;
        private int _length;
        private Boolean _caseSensitive;

        private Boolean[] _flags;

        private MyWord() { }

        public MyWord(String theWord, Boolean caseSensitive = false)
        {
            _theWord = theWord.Trim();
            if (String.IsNullOrEmpty(_theWord))
                throw new Exception("The word cannot be null or empty.");

            _length = _theWord.Count();

            _caseSensitive = caseSensitive;
            if (!_caseSensitive)
                _theWord.ToLower();

            _flags = new Boolean[_length];
        }

        public Boolean IsWordFound()
        {
            return _flags[_length - 1];
        }

        public Boolean QueueNewChar(Char newChar)
        {
            int counter = -1;

            foreach (Boolean charFound in _flags)
            {
                counter++;

                if (charFound)
                    continue;

                if (_theWord[counter].Equals(newChar))
                {
                    _flags[counter] = true;
                    return this.IsWordFound();
                }

                _flags = new Boolean[_length];
                return false;
            }

            return true;
        }

        //private String _theWord;
        //private int _wordLength;
        //private Boolean _caseSesitive;

        //private Queue<Char> _current;

        //private MyWord() { }

        //public MyWord(String theWord, Boolean caseSesitive = false)
        //{
        //    _theWord = theWord.Trim();

        //    if (String.IsNullOrEmpty(theWord))
        //        throw new Exception("The word cannot be null or empty!");

        //    _wordLength = theWord.Count();

        //    _caseSesitive = caseSesitive;
        //    if (!_caseSesitive)
        //        _theWord = _theWord.ToLower();

        //    _current = new Queue<Char>();
        //}

        //public Boolean IsWordMatch()
        //{
        //    if (_current.Count == _wordLength)
        //        if (new String(_current.ToArray()).Equals(_theWord))
        //            return true;
        //    return false;
        //}

        //public Boolean IsWordMatch(Char newChar)
        //{
        //    if (_current.Count > _wordLength)
        //        throw new Exception(String.Format("Current queue <{0}> has {1} of chars, more than the word <{2}> should be.", new String(_current.ToArray()), _current.Count, _theWord));

        //    if (_current.Count == _wordLength)
        //        _current.Dequeue();

        //    if (_caseSesitive)
        //        _current.Enqueue(newChar);
        //    else
        //        _current.Enqueue(Char.ToLower(newChar));

        //    return this.IsWordMatch();
        //}
    }
}
