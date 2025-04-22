import { yupResolver } from "@hookform/resolvers/yup";
import React from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as Yup from "yup";
interface Props {
  symbol: string;
  handleCommentSubmit: (e: CompanyInputForms) => Promise<any>;
}

type CompanyInputForms = {
  title: string;
  content: string;
};
const validation = Yup.object().shape({
  title: Yup.string()
    .min(5, "Must be atleast 5 characters long")
    .max(10, "Title can't be too long")
    .required("Title is required"),
  content: Yup.string()
    .min(30, "Must be atleast 30 characters long")
    .max(280, "No one's reading all of that")
    .required("Content is required"),
});

const StockCommentForm = ({ symbol, handleCommentSubmit }: Props) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<CompanyInputForms>({ resolver: yupResolver(validation) });
  const onSubmit = (data: CompanyInputForms) => {
    handleCommentSubmit(data)
      .then(() => {
        reset();
      })
      .catch((err) => {
        toast.warning(err);
      });
  };

  return (
    <form className="mt-4 ml-4" onSubmit={handleSubmit(onSubmit)}>
      <input
        type="text"
        id="title"
        className="mb-3 bg-white border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
        placeholder="Title"
        {...register("title")}
      />
      {errors.title && <p>{errors.title.message}</p>}
      <div className="py-2 px-4 mb-4 bg-white rounded-lg rounded-t-lg border border-gray-200 dark:bg-gray-800 dark:border-gray-700">
        <label htmlFor="comment" className="sr-only">
          Your comment
        </label>
        <textarea
          id="comment"
          rows={6}
          className="px-0 w-full text-sm text-gray-900 border-0 focus:ring-0 focus:outline-none dark:text-white dark:placeholder-gray-400 dark:bg-gray-800"
          placeholder="Write a comment..."
          {...register("content")}
        ></textarea>
      </div>
      {errors.content && <p>{errors.content.message}</p>}
      <button
        type="submit"
        className="inline-flex items-center py-2.5 px-4 text-xs font-medium text-center text-white bg-lightGreen rounded-lg focus:ring-4 focus:ring-primary-200 dark:focus:ring-primary-900 hover:bg-primary-800"
      >
        Post comment
      </button>
    </form>
  );
};

export default StockCommentForm;
