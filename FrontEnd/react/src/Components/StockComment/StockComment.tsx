import React, { useEffect, useState } from "react";
import StockCommentForm from "./StockCommentForm/StockCommentForm";
import {
  createCommentAPI,
  getAllCommentsAPI,
} from "../../Services/CommentService";
import { CommentGet } from "../../Models/Comment";
import StockCommentList from "../StockCommentList/StockCommentList";
import { toast } from "react-toastify";

interface Props {
  symbol: string;
}

type CompanyInputForms = {
  title: string;
  content: string;
};

const StockComment = ({ symbol }: Props) => {
  const [loading, setLoading] = useState<boolean>();
  const [comments, setComments] = useState<CommentGet[] | null>(null);

  useEffect(() => {
    fetchAllComments();
  }, []);

  const fetchAllComments = async () => {
    setLoading(true);
    const fetchedComments = await getAllCommentsAPI(symbol);
    setLoading(false);
    setComments(fetchedComments?.data!);
  };

  const handleComment = (e: CompanyInputForms) => {
    return createCommentAPI(e.title, e.content, symbol)
      .then((res) => {
        toast.success("Comment successfully added");
        fetchAllComments();
        return res;
      })
      .catch((err) => toast.warning(err));
  };

  return (
    <div className="flex flex-col">
      <div className="mt-4">
        <StockCommentList comments={comments} />
      </div>
      <StockCommentForm symbol={symbol} handleCommentSubmit={handleComment} />
    </div>
  );
};

export default StockComment;
