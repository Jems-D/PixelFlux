import React from "react";
import { ClipLoader } from "react-spinners";
import "./Spinner.css";
interface Props {
  isLoading?: boolean;
}

const Spinner = ({ isLoading = true }: Props) => {
  return (
    <div id="clip-loader-spinner">
      <ClipLoader
        color="#D6B4FC"
        aria-label="Loading Spinner"
        size="45"
        data-testid="loader"
        loading={isLoading}
      />
    </div>
  );
};

export default Spinner;
