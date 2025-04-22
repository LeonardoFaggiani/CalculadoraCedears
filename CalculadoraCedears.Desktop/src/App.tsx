import "./App.css";
import { RouterProvider } from "react-router-dom";
import router from './router/router';
function App() {
  
  return (
    <div className="flex flex-col min-h-screen">
      <main className="flex-1 container mx-auto p-8">
      <RouterProvider router={router} />
      </main>
    </div>
  );
}

export default App;
