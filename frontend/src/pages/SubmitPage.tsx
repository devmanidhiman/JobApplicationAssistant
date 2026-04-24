import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { Button } from '@/components/ui/button'
import type { PipelineResult } from '../types/api'
import { API_BASE_URL } from '@/config'

export default function SubmitPage() {
  const [jobDescription, setJobDescription] = useState('')
  const [resumeText, setResumeText] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const navigate = useNavigate()

  async function handleSubmit() {
    if (!jobDescription.trim() || !resumeText.trim()) {
      setError('Both fields are required.')
      return
    }

    setLoading(true)
    setError(null)

    try {
      const response = await fetch(`${API_BASE_URL}/api/jobapplication/analyze`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ jobDescription, resumeText }),
      })

      if (!response.ok) throw new Error(`Request failed: ${response.status}`)

      const data: PipelineResult = await response.json()
      console.log(data)
      navigate('/history')
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Something went wrong.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-2xl font-bold">Analyze Job Application</h1>
        <p className="text-muted-foreground mt-1">Paste a job description and your resume to run the pipeline.</p>
      </div>

      <div className="flex flex-col gap-2">
        <label className="font-medium text-sm">Job Description</label>
        <textarea
          className="w-full h-48 p-3 rounded-md border bg-background text-sm resize-none focus:outline-none focus:ring-2 focus:ring-ring"
          placeholder="Paste the job description here..."
          value={jobDescription}
          onChange={(e) => setJobDescription(e.target.value)}
        />
      </div>

      <div className="flex flex-col gap-2">
        <label className="font-medium text-sm">Resume</label>
        <textarea
          className="w-full h-48 p-3 rounded-md border bg-background text-sm resize-none focus:outline-none focus:ring-2 focus:ring-ring"
          placeholder="Paste your resume text here..."
          value={resumeText}
          onChange={(e) => setResumeText(e.target.value)}
        />
      </div>

      {error && <p className="text-destructive text-sm">{error}</p>}

      <Button onClick={handleSubmit} disabled={loading} className="w-fit">
        {loading ? 'Analyzing...' : 'Analyze'}
      </Button>
    </div>
  )
}