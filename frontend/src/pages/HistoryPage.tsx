import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import type { JobApplicationSummary } from '../types/api'
import { API_BASE_URL } from '@/config'

export default function HistoryPage() {
  const [applications, setApplications] = useState<JobApplicationSummary[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const navigate = useNavigate()

  useEffect(() => {
    fetch(`${API_BASE_URL}/api/jobapplication`)
      .then(res => {
        if (!res.ok) throw new Error(`Request failed: ${res.status}`)
        return res.json()
      })
      .then((data: JobApplicationSummary[]) => setApplications(data))
      .catch(err => setError(err.message))
      .finally(() => setLoading(false))
  }, [])

  if (loading) return <p className="text-muted-foreground">Loading...</p>
  if (error) return <p className="text-destructive">{error}</p>
  if (applications.length === 0) return <p className="text-muted-foreground">No applications yet.</p>

  return (
    <div className="flex flex-col gap-4">
      <h1 className="text-2xl font-bold">History</h1>
      {applications.map(app => (
        <div
          key={app.id}
          onClick={() => navigate(`/history/${app.id}`)}
          className="border rounded-lg p-4 cursor-pointer hover:bg-muted transition-colors"
        >
          <div className="flex items-center justify-between mb-2">
            <span className={`text-xs font-medium px-2 py-1 rounded-full ${
              app.status === 'completed' ? 'bg-green-100 text-green-700' :
              app.status === 'failed' ? 'bg-red-100 text-red-700' :
              'bg-yellow-100 text-yellow-700'
            }`}>
              {app.status}
            </span>
            <span className="text-xs text-muted-foreground">
              {new Date(app.createdAt).toLocaleString()}
            </span>
          </div>
          <p className="text-sm line-clamp-2 text-muted-foreground">{app.jobDescription}</p>
        </div>
      ))}
    </div>
  )
}