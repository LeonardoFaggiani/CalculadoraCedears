import "./App.css";
import { RouterProvider } from "react-router-dom";
import router from "./router/router";

function App() {
  return (
    <div className="min-h-screen">
      <main className="flex-1 mx-auto">
        <RouterProvider router={router} />
      </main>
    </div>
  );
}

export default App;
