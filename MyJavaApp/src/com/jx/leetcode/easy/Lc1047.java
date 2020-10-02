package com.jx.leetcode.easy;

import java.util.Stack;

public class Lc1047 {

    // 14ms, 40.2MB
    public String applyWithStack(final String input) {

        final char[] list = input.toCharArray();
        final Stack<Character> stack = new Stack<>();

        for (int i = 0; i < input.length(); i++) {
            if (!stack.isEmpty() && list[i] == stack.peek()) {
                stack.pop();
            } else {
                stack.push(list[i]);
            }
        }

        final char[] result = new char[stack.size()];
        for (int i = stack.size() - 1; i >= 0; i--) {
            result[i] = stack.pop();
        }

        return new String(result);
    }

    // 7ms, 39.8MB
    public String applyWithStringBuffer(final String input) {

        final char[] list = input.toCharArray();
        final StringBuilder sb = new StringBuilder();

        for (char c : list) {
            if (sb.length() != 0 && c == sb.charAt(sb.length() - 1)) {
                sb.deleteCharAt(sb.length() - 1);
            } else {
                sb.append(c);
            }
        }

        return sb.toString();
    }

    // 3ms, 39.7MB
    public String applyWithTwoPointers(final String input) {

        final char[] list = input.toCharArray();
        final char[] result = new char[input.length()];
        int i = 0, j = -1;

        for (; i < list.length; i++) {
            if (j == -1) {
                result[++j] = list[i];
            } else if (list[i] != result[j]) {
                result[++j] = list[i];
            } else {
                j--;
            }
        }

        return new String(result, 0, j + 1);
    }
}
