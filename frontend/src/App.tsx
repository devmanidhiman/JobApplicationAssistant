import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import Layout from './components/Layout'
import SubmitPage from './pages/SubmitPage'
import HistoryPage from './pages/HistoryPage'
import DetailPage from './pages/DetailPage'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route path="/" element={<Navigate to="/submit" replace />} />
          <Route path="/submit" element={<SubmitPage />} />
          <Route path="/history" element={<HistoryPage />} />
          <Route path="/history/:id" element={<DetailPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App