import axios from "axios";
import { errorHandler } from "../Helpers/ErrorHandler";
import { CommentGet, CommentPost } from "../Models/Comment";

const api = "http://localhost:5005/api/comment/";

export const createCommentAPI = async (
  title: string,
  content: string,
  symbol: string
) => {
  try {
    const data = await axios.post<CommentPost>(api + `${symbol}`, {
      title: title,
      content: content,
    });
    return data;
  } catch (err) {
    errorHandler(err);
  }
};

export const getAllCommentsAPI = async (symbol: string) => {
  try {
    const data = await axios.get<CommentGet[]>(`${api}?Symbol=${symbol}`);
    return data;
  } catch (err) {
    errorHandler(err);
  }
};
