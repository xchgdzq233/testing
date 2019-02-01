using log4net;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundamentals.TestOnlineJudges.TestLeetCode
{
    [TestFixture]
    public class TestLeetCodeEasy
    {
        private ILog logger = LogManager.GetLogger(typeof(TestLeetCodeEasy));

        #region "1 two sum"
        private int[] _1TwoSum(int[] nums, int target)
        {
            int[] result = new int[2];

            Dictionary<int, List<int>> map = new Dictionary<int, List<int>>();
            for (int i = 0; i <= nums.Length - 1; i++)
            {
                if (!map.ContainsKey(nums[i]))
                    map.Add(nums[i], new List<int>());
                map[nums[i]].Add(i);
            }

            List<int> numAsKeys = map.Keys.ToList();
            for (int i = 0; i <= numAsKeys.Count - 1; i++)
            {
                if (map.ContainsKey(target - numAsKeys[i]))
                {
                    result[0] = map[numAsKeys[i]][0];
                    if (numAsKeys[i] * 2 == target)
                    {
                        if (map[numAsKeys[i]].Count < 2)
                            continue;
                        else
                            result[1] = map[target - numAsKeys[i]][1];
                    }
                    else
                        result[1] = map[target - numAsKeys[i]][0];
                    break;
                }
            }

            return result;
        }
        #endregion

        #region "13 Roman to Integer"
        private int _13RomanToInteger(string s)
        {
            Dictionary<char, int> map = new Dictionary<char, int>();
            map.Add('I', 1);
            map.Add('V', 5);
            map.Add('X', 10);
            map.Add('L', 50);
            map.Add('C', 100);
            map.Add('D', 500);
            map.Add('M', 1000);

            int result = 0, last = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                int current = map[s[i]];
                if (current < last)
                    result -= current;
                else
                {
                    result += current;
                    last = current;
                }
            }

            return result;
        }
        #endregion

        #region "20 Valid Parentheses"
        private bool _20ValidParentheses(string s)
        {
            if (s.Length == 0) return true;

            Stack<char> st = new Stack<char>();
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (s[i] == '(')
                    st.Push(')');
                else if (s[i] == '[')
                    st.Push(']');
                else if (s[i] == '{')
                    st.Push('}');
                else
                {
                    if (st.Count == 0 || st.Pop() != s[i])
                        return false;
                }
            }

            return st.Count == 0;
        }

        private bool _20MyValidParentheses(string s)
        {
            Stack<char> st = new Stack<char>();
            for (int i = 0; i <= s.Length - 1; i++)
            {
                if (s[i] == '(' || s[i] == '[' || s[i] == '{')
                    st.Push(s[i]);
                else
                {
                    if (st.Count == 0)
                        return false;

                    char last = st.Pop();
                    if ((s[i] == ')' && last != '(') || (s[i] == ']' && last != '[') || (s[i] == '}' && last != '{'))
                        return false;
                }
            }
            return st.Count == 0;
        }
        #endregion

        #region "21 Merge Two Sorted Lists"
        private ListNode _21MergeTwoSortedLists(ListNode l1, ListNode l2)
        {
            return this.MergeTwoLists(l1, l2);
        }

        private ListNode MergeTwoLists(ListNode l1, ListNode l2)
        {
            if (l1 == null) return l2;
            if (l2 == null) return l1;

            if (l1.val < l2.val)
            {
                l1.next = MergeTwoLists(l1.next, l2);
                return l1;
            }
            l2.next = MergeTwoLists(l1, l2.next);
            return l2;
        }

        private ListNode _21MyMergeTwoSortedLists(ListNode l1, ListNode l2)
        {
            if (l1 == null) return l2;
            if (l2 == null) return l1;

            ListNode dummy = new ListNode(-1);
            ListNode current = dummy;

            while (l1 != null && l2 != null)
            {
                if (l1.val < l2.val)
                {
                    current.next = l1;
                    l1 = l1.next;
                }
                else
                {
                    current.next = l2;
                    l2 = l2.next;
                }
                current = current.next;
            }

            if (l1 != null)
                current.next = l1;
            if (l2 != null)
                current.next = l2;

            return dummy.next;
        }
        #endregion

        #region "206 Reverse Linked List"
        private ListNode _206ReverseLinkedList(ListNode head)
        {
            return this.ReverseList(head);
        }

        private ListNode ReverseList(ListNode head)
        {
            if (head == null || head.next == null)
                return head;

            ListNode dummy = ReverseList(head.next);
            head.next.next = head;
            head.next = null;
            return dummy;
        }

        private ListNode _206ReverseLinkedListIteratively(ListNode head)
        {
            if (head == null) return null;

            Stack<ListNode> s = new Stack<ListNode>();
            while (head != null)
            {
                s.Push(head);
                head = head.next;
            }

            ListNode dummy = new ListNode(-1);
            head = s.Pop();
            dummy.next = head;
            while (s.Count != 0)
            {
                head.next = s.Pop();
                head = head.next;
            }
            head.next = null;
            return dummy.next;
        }
        #endregion

        #region "88 Merge Sorted Array"
        private void _88MergeSortedArray(int[] nums1, int m, int[] nums2, int n)
        {
            m--;
            n--;
            for (int i = nums1.Length - 1; m >= 0 && n >= 0; i--)
            {
                if (nums1[m] > nums2[n])
                    nums1[i] = nums1[m--];
                else
                    nums1[i] = nums2[n--];
            }

            for (int i = 0; i <= n; i++)
                nums1[i] = nums2[i];
        }
        #endregion

        #region "438 Find All Anagrams in a String"
        private IList<int> _438FindAllAnagramsInAString(string s, string p)
        {
            Dictionary<char, int> map = new Dictionary<char, int>();
            foreach(char c in p)
            {
                if(!map.ContainsKey(c))
                {
                    map.Add(c, 1);
                }
                else
                {
                    map[c]++;
                }
            }

            IList<int> res = new List<int>();
            int start = 0, end = 0, count = map.Keys.Count, len = p.Length;

            while(end < s.Length)
            {
                if (map.ContainsKey(s[end]))
                {
                    if (--map[s[end]] == 0)
                    {
                        count--;
                    }
                }

                if (count == 0)
                {
                    res.Add(start);
                }

                end++;
                if (end - start == len)
                {
                    if (map.ContainsKey(s[start]))
                    {
                        if (map[s[start]]++ == 0)
                        {
                            count++;
                        }
                    }
                    start++;
                }
            }

            return res;
        }
        #endregion

        #region "680. Valid Palindrome II"
        private bool _80ValidPalindrome2(string s)
        {
            this.s = s;
            return ValidPalindromeSub(0, s.Length - 1, false);
        }

        private string s;

        private bool ValidPalindromeSub(int left, int right, bool skipped)
        {
            int l = -1, r = -1;
            while (left < right)
            {
                if (s[left] != s[right])
                {
                    l = left;
                    r = right;
                    break;
                }
                left++;
                right--;
            }

            // mismatch never happened
            if (l == -1)
            {
                return true;
            }

            // mismatch happened, but haven't skipped before, so skipping
            if (!skipped)
            {
                return ValidPalindromeSub(l + 1, r, true) || ValidPalindromeSub(l, r - 1, true);
            }

            // mismatch happened, but skipped before
            return false;
        }

        #endregion

        [Test]
        public void TestEasy()
        {
            #region "680. Valid Palindrome II"
            Assert.False(_80ValidPalindrome2("cxcaac"));
            #endregion

            #region "438 Find All Anagrams in a String"
            //Assert.That(this._438FindAllAnagramsInAString("cbaebabacd", "abc"), Is.EqualTo(new List<int>() { 0, 6 }));
            //Assert.That(this._438FindAllAnagramsInAString("abab", "ab"), Is.EqualTo(new List<int>() { 0, 1, 2 }));
            //Assert.That(this._438FindAllAnagramsInAString("cbaebabacd", "abeb"), Is.EqualTo(new List<int>() { 1, 3 }));
            #endregion

            #region "88 Merge Sorted Array"
            //int[] nums1 = null;

            //nums1 = new int[] { 2, 5, 6, 0, 0, 0 };
            //this._88MergeSortedArray(nums1, 3, new int[] { 1, 2, 3 }, 3);
            //Assert.That(nums1, Is.EquivalentTo(new int[] { 1, 2, 2, 3, 5, 6 }));

            //nums1 = new int[] { 1, 2, 3, 0, 0, 0 };
            //this._88MergeSortedArray(nums1, 3, new int[] { 2, 5, 6 }, 3);
            //Assert.That(nums1, Is.EquivalentTo(new int[] { 1, 2, 2, 3, 5, 6 }));
            #endregion

            #region "206 Reverse Linked List"
            //Assert.That(this._206ReverseLinkedList(new ListNode(new List<int>() { 1, 2, 3, 4, 5 })).GetThisAndAfterInString(), Is.EqualTo("5 4 3 2 1 "));
            #endregion

            #region "21 Merge Two Sorted Lists"
            //Assert.That(this._21MergeTwoSortedLists(new ListNode(new List<int>() { 1, 2, 4 }), new ListNode(new List<int>() { 1, 3, 4 })).GetThisAndAfterInString(), Is.EqualTo("1 1 2 3 4 4 "));
            #endregion

            #region "20 Valid Parentheses"
            //Assert.False(this._20ValidParentheses("["));
            //Assert.False(this._20ValidParentheses("]"));
            //Assert.True(this._20ValidParentheses("()"));
            //Assert.True(this._20ValidParentheses("()[]{}"));
            //Assert.False(this._20ValidParentheses("(]"));
            //Assert.False(this._20ValidParentheses("([)]"));
            //Assert.True(this._20ValidParentheses("{[]}"));
            #endregion

            #region "13 Roman to Integer"
            //Assert.That(this._13RomanToInteger("III"), Is.EqualTo(3));
            //Assert.That(this._13RomanToInteger("IV"), Is.EqualTo(4));
            //Assert.That(this._13RomanToInteger("IX"), Is.EqualTo(9));
            //Assert.That(this._13RomanToInteger("LVIII"), Is.EqualTo(58));
            //Assert.That(this._13RomanToInteger("MCMXCIV"), Is.EqualTo(1994));
            #endregion

            #region "1 two sum"
            //Assert.That(this._1TwoSum(new int[] { 3, 2, 4 }, 6), Is.EquivalentTo(new int[] { 1, 2 }));
            //Assert.That(this._1TwoSum(new int[] { -1, -2, -3, -4, -5 }, -8), Is.EquivalentTo(new int[] { 2, 4 }));
            //Assert.That(this._1TwoSum(new int[] { 0, 4, 3, 0 }, 0), Is.EqualTo(new int[] { 0, 3 }));
            //Assert.That(this._1TwoSum(new int[] { 3, 3 }, 6), Is.EqualTo(new int[] { 0, 1 }));
            //Assert.That(this._1TwoSum(new int[] { 2, 7, 11, 15 }, 9), Is.EqualTo(new int[] { 0, 1 }));
            #endregion
        }
    }
}
