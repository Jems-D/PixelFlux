import React, { SyntheticEvent } from "react";

interface Props {
  onDeletePortfolio: (e: SyntheticEvent) => void;
  portfolio: string;
}

const DeletePortfolio = ({ onDeletePortfolio, portfolio }: Props) => {
  return (
    <form onSubmit={onDeletePortfolio}>
      <input readOnly={true} hidden={true} value={portfolio}></input>
      <button
        type="submit"
        className="block w-full py-3 text-white duration-200 border-2 rounded-lg bg-red-500 hover:text-red-500 hover:bg-white border-red-500"
      >
        X
      </button>
    </form>
  );
};

export default DeletePortfolio;
