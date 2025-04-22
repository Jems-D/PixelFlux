import React from "react";
import { CommentGet } from "../../Models/Comment";
import StockCommentListItem from "./StockCommentListItem";

interface Props {
  comments: CommentGet[] | null;
}

const StockCommentList = ({ comments }: Props) => {
  return (
    <>
      {comments
        ? comments.map((comment, index) => {
            return <StockCommentListItem comment={comment} key={index} />;
          })
        : ""}
    </>
  );
};

export default StockCommentList;
