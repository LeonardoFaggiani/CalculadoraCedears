"use client";

import { useState, useEffect } from "react";
import { Card, CardHeader, CardTitle } from "../ui/card";
import { Search, Download, CheckCircle, Clock } from "lucide-react";
import { check } from "@tauri-apps/plugin-updater";
import { invoke } from "@tauri-apps/api/core";

export default function CheckUpdates() {
  const [updateStatus, setUpdateStatus] = useState<
    "checking" | "available" | "upToDate"
  >("checking");

  const checkForUpdates = async () => {
    setUpdateStatus("checking");
    try {
      const update = await check();

      if (update === null) {
        setUpdateStatus("upToDate");
        await invoke("show_main_menu");
        return;
      }

      setUpdateStatus("available");
      await update.downloadAndInstall();

    } catch (error) {
      console.error("Fallo la búsqueda de actualización", error);
      setUpdateStatus("upToDate");
      await invoke("show_main_menu");
    }

    await invoke("relaunch");
  };

  const showSplashScreen = async () => {
    await invoke("show_splash_screen");
  };

  useEffect(() => {
    showSplashScreen();
    setTimeout(() => {
      checkForUpdates();
    }, 2000);
  }, []);

  return (
<div className="w-screen h-screen bg-gray-100 flex items-center justify-center">
      <Card className="w-full h-full max-w-sm bg-white shadow-lg rounded-2xl pt-16">
        <CardHeader className="text-center pb-4">
          <div className="flex justify-center mb-4">
            {updateStatus === "checking" && (
              <>
                <div className="bg-emerald-500 rounded-2xl p-4 mb-2">
                  <Clock className="h-8 w-8 text-white" />
                </div>
                <div className="bg-emerald-100 rounded-full p-4 -ml-4 mt-8">
                  <Search className="h-8 w-8 text-emerald-600 animate-bounce" />
                </div>
              </>
            )}

            {updateStatus === "available" && (
              <>
                <div className="bg-emerald-500 rounded-2xl p-4 mb-2">
                  <Download className="h-8 w-8 text-white" />
                </div>
                <div className="bg-emerald-100 rounded-full p-4 -ml-4 mt-8">
                  <CheckCircle className="h-8 w-8 text-emerald-600" />
                </div>
              </>
            )}

            {updateStatus === "upToDate" && (
              <>
                <div className="bg-emerald-500 rounded-2xl p-4 mb-2">
                  <CheckCircle className="h-8 w-8 text-white" />
                </div>
                <div className="bg-emerald-100 rounded-full p-4 -ml-4 mt-8">
                  <CheckCircle className="h-8 w-8 text-emerald-600" />
                </div>
              </>
            )}
          </div>

          <CardTitle className="text-xl font-semibold text-gray-900">
            {updateStatus === "checking" && "Buscando actualizaciones..."}
            {updateStatus === "available" && "Descargando actualización"}
            {updateStatus === "upToDate" && "¡App Actualizada!"}
          </CardTitle>
        </CardHeader>
      </Card>
    </div>
  );
}
