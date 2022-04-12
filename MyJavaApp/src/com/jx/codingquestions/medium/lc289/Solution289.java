package com.jx.codingquestions.medium.lc289;

import com.jx.annotations.Topics;
import com.jx.data.Topic;

/**
 * https://leetcode.com/problems/game-of-life/
 * Related questions: {@link com.jx.codingquestions.medium.lc73.Solution73}
 */
@Topics(value = { Topic.Array, Topic.Matrix })
public class Solution289 {

    public void gameOfLife(int[][] board) {

        for (int row = 0; row < board.length; row++) {
            for (int col = 0; col < board[0].length; col++) {
                board[row][col] += getCellNextGen(board, row, col) * 10;
            }
        }

        for (int row = 0; row < board.length; row++) {
            for (int col = 0; col < board[0].length; col++) {
                board[row][col] /= 10;
            }
        }
    }

    private int getCellNextGen(int[][] board, final int row, final int col) {

        int result = 0;
        result += getCellVal(board, row - 1, col - 1);
        result += getCellVal(board, row - 1, col);
        result += getCellVal(board, row - 1, col + 1);
        result += getCellVal(board, row, col + 1);
        result += getCellVal(board, row + 1, col + 1);
        result += getCellVal(board, row + 1, col);
        result += getCellVal(board, row + 1, col - 1);
        result += getCellVal(board, row, col - 1);

        if (isValidCell(board, row, col)) {
            if (result == 3) {
                return 1;
            }
            if (result == 2 && board[row][col] == 1) {
                return 1;
            }
        }
        return 0;
    }

    private int getCellVal(int[][] board, final int row, final int col) {

        return isValidCell(board, row, col) ? board[row][col] % 10 : 0;
    }

    private boolean isValidCell(int[][] board, final int row, final int col) {

        return (row >= 0 && row < board.length) && (col >= 0 && col < board[0].length);
    }
}
