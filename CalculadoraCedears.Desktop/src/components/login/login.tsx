import React, { useState, useMemo } from "react";
import { Button } from "../ui/button";
import { CardContent, CardHeader, CardTitle, Card } from "../ui/card";

import { login } from "@/services/auth.service";
import { useNavigate } from "react-router-dom";
import { ToastService } from "@/services/toast.service";
import { error as LogError } from "@tauri-apps/plugin-log";

const GoogleIcon: React.FC = () => (
  <svg
    className="mr-2 h-4 w-4"
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 48 48"
    width="48px"
    height="48px"
  >
    <path
      fill="#FFC107"
      d="M43.611,20.083H42V20H24v8h11.303c-1.649,4.657-6.08,8-11.303,8c-6.627,0-12-5.373-12-12c0-6.627,5.373-12,12-12c3.059,0,5.842,1.154,7.961,3.039l5.657-5.657C34.046,6.053,29.268,4,24,4C12.955,4,4,12.955,4,24c0,11.045,8.955,20,20,20c11.045,0,20-8.955,20-20C44,22.659,43.862,21.35,43.611,20.083z"
    />
    <path
      fill="#FF3D00"
      d="M6.306,14.691l6.571,4.819C14.655,15.108,18.961,12,24,12c3.059,0,5.842,1.154,7.961,3.039l5.657-5.657C34.046,6.053,29.268,4,24,4C16.318,4,9.656,8.337,6.306,14.691z"
    />
    <path
      fill="#4CAF50"
      d="M24,44c5.166,0,9.86-1.977,13.409-5.192l-6.19-5.238C29.211,35.091,26.715,36,24,36c-5.202,0-9.619-3.317-11.283-7.946l-6.522,5.025C9.505,39.556,16.227,44,24,44z"
    />
    <path
      fill="#1976D2"
      d="M43.611,20.083H42V20H24v8h11.303c-0.792,2.237-2.231,4.166-4.087,5.571l0.001-0.001l6.19,5.238C39.712,34.461,44,29.697,44,24C44,22.659,43.862,21.35,43.611,20.083z"
    />
  </svg>
);

const SpinnerIcon: React.FC = () => (
  <span className="animate-spin mr-2">⟳</span>
);

type LoginProvider = "google";

const Login: React.FC = () => {
  const navigate = useNavigate();
  const [isGoogleLoading, setIsGoogleLoading] = useState<boolean>(false);
  const isLoading = useMemo(() => isGoogleLoading, [isGoogleLoading]);

  const handleLogin = async (provider: LoginProvider) => {
    try {
      setIsGoogleLoading(true);

      await login(provider);
      navigate("/home");
    } catch (error: any) {
      if (error instanceof Error) {
        LogError(`Mensaje de error:${error.message}`);
        ToastService.error(`Mensaje de error:${error.message}`);
      } else {
        LogError(`Error inesperado:${error}`);
        ToastService.error(`Error inesperado:${error}`);
      }
    } finally {
      setIsGoogleLoading(false);
    }
  };

  const loginWithGoogle = () => handleLogin("google");

  return (
    <div className="flex items-center justify-center min-h-screen">
      <Card className="w-full max-w-sm">
        <CardHeader>
          <CardTitle className="text-2xl font-bold text-center">
            Bienvenido a Calculadora Cedears
          </CardTitle>
        </CardHeader>
        <CardContent className="grid gap-4">
          <Button
            variant="outline"
            className="cursor-pointer"
            onClick={loginWithGoogle}
            disabled={isLoading}
          >
            {!isGoogleLoading ? <GoogleIcon /> : <SpinnerIcon />}
            Google Login
          </Button>
        </CardContent>
      </Card>
    </div>
  );
};

export default Login;
