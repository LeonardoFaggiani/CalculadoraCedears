import { toastBaseStyle } from "@/lib/utils";
import { toast, ToastT } from "sonner";

export type ToastOptions = {
  duration?: number;
  position?:
    | "top-center"
    | "top-right"
    | "top-left"
    | "bottom-center"
    | "bottom-right"
    | "bottom-left";
  icon?: React.ReactNode;
  id?: string;
  onDismiss?: (toast: ToastT) => void;
  onAutoClose?: (toast: ToastT) => void;
  cancel?: React.ReactNode;
  cancelButtonStyle?: React.CSSProperties;
  action?: {
    label: string;
    onClick: () => void;
    style?: React.CSSProperties;
  };
};

export const ToastService = {
  success: (message: string, options?: ToastOptions) => {
    return toast.success(message, options);
  },
  error: (message: string, options?: ToastOptions) => {
    return toast.error(message, options);
  },
  info: (message: string, options?: ToastOptions) => {
    return toast.info(message, options);
  },
  warning: (message: string, options?: ToastOptions) => {
    return toast.warning(message, options);
  },
  custom: (
    title: string,
    message?: string,
    options?: ToastOptions & {
      type?: "success" | "error" | "info" | "warning" | "default";
    }
  ) => {
    const { type = "default", ...restOptions } = options || {};

    const selectedStyle = toastBaseStyle[type] || {};

    return toast(title, {
      description: message,
      style: selectedStyle,
      classNames: {
        description:
          type === "success"
            ? "!text-green-900"
            : type === "error"
            ? "!text-red-900"
            : type === "info"
            ? "!text-blue-900"
            : type === "warning"
            ? "!text-orange-900"
            : undefined,
      },
      ...restOptions,
    });
  },
  promise: <T>(
    promise: Promise<T>,
    messages: {
      loading: string;
      success: string | ((data: T) => string);
      error: string | ((error: Error) => string);
    },
    options = {}
  ) => {
    const toastId = toast.loading(messages.loading, {
      className: "bg-gray-100 text-black",
      ...options,
    });

    promise
      .then((data) => {
        const successMessage =
          typeof messages.success === "function"
            ? messages.success(data)
            : messages.success;

        toast.success(successMessage, {
          id: toastId,
          style: toastBaseStyle["success"],
          className: "bg-green-100 text-green-800",
          ...options,
        });
        return data;
      })
      .catch((error) => {
        const errorMessage =
          typeof messages.error === "function"
            ? messages.error(error)
            : messages.error;

        toast.error(errorMessage, {
          id: toastId,
          style: toastBaseStyle["error"],
          className: "bg-red-100 text-red-800",
          ...options,
        });
        throw error;
      });

    return promise;
  },
};
