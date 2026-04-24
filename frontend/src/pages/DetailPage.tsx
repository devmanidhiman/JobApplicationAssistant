import { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import type { JobApplicationDetail } from '../types/api'
import { API_BASE_URL } from '../config'

export default function DetailPage() {
  const { id } = useParams<{ id: string }>()
  const [detail, setDetail] = useState<JobApplicationDetail | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    fetch(`${API_BASE_URL}/api/jobapplication/${id}`)
      .then(res => {
        if (!res.ok) throw new Error(`Request failed: ${res.status}`)
        return res.json()
      })
      .then((data: JobApplicationDetail) => setDetail(data))
      .catch(err => setError(err.message))
      .finally(() => setLoading(false))
  }, [id])

  if (loading) return <p className="text-muted-foreground">Loading...</p>
  if (error) return <p className="text-destructive">{error}</p>
  if (!detail) return <p className="text-muted-foreground">Not found.</p>

  return (
    <div className="flex flex-col gap-8">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Pipeline Result</h1>
        <span className={`text-xs font-medium px-2 py-1 rounded-full ${
          detail.status === 'completed' ? 'bg-green-100 text-green-700' :
          detail.status === 'failed' ? 'bg-red-100 text-red-700' :
          'bg-yellow-100 text-yellow-700'
        }`}>
          {detail.status}
        </span>
      </div>

      {detail.skillExtraction && (
        <section className="flex flex-col gap-3">
          <h2 className="text-lg font-semibold border-b pb-2">Step 1 — Skill Extraction</h2>
          <p><span className="font-medium">Job Title:</span> {detail.skillExtraction.jobTitle}</p>
          <p><span className="font-medium">Seniority:</span> {detail.skillExtraction.seniority}</p>
          <p><span className="font-medium">Required Skills:</span> {detail.skillExtraction.requiredSkills.join(', ')}</p>
          <p><span className="font-medium">Nice to Have:</span> {detail.skillExtraction.niceToHaveSkills.join(', ')}</p>
          <p><span className="font-medium">Keywords:</span> {detail.skillExtraction.keywords.join(', ')}</p>
        </section>
      )}

      {detail.resumeMatch && (
        <section className="flex flex-col gap-3">
          <h2 className="text-lg font-semibold border-b pb-2">Step 2 — Resume Match</h2>
          <p><span className="font-medium">Match Score:</span> {detail.resumeMatch.matchScore}%</p>
          <p><span className="font-medium">Matched Skills:</span> {detail.resumeMatch.matchedSkills.join(', ')}</p>
          <p><span className="font-medium">Missing Skills:</span> {detail.resumeMatch.missingSkills.join(', ')}</p>
          <p><span className="font-medium">Summary:</span> {detail.resumeMatch.summary}</p>
        </section>
      )}

      {detail.resumeRewrite && (
        <section className="flex flex-col gap-3">
          <h2 className="text-lg font-semibold border-b pb-2">Step 3 — Resume Rewrite</h2>
          {detail.resumeRewrite.rewrittenBullets.map((bullet, i) => (
            <div key={i} className="flex flex-col gap-1 border rounded-md p-3">
              <p className="text-sm text-muted-foreground line-through">{bullet.original}</p>
              <p className="text-sm font-medium">{bullet.rewritten}</p>
              <p className="text-xs text-muted-foreground">{bullet.reasoning}</p>
            </div>
          ))}
        </section>
      )}

      {detail.coverLetter && (
        <section className="flex flex-col gap-3">
          <h2 className="text-lg font-semibold border-b pb-2">Step 4 — Cover Letter</h2>
          <p className="text-sm text-muted-foreground italic">{detail.coverLetter.strategyNotes}</p>
          <pre className="whitespace-pre-wrap text-sm bg-muted p-4 rounded-lg">{detail.coverLetter.coverLetter}</pre>
        </section>
      )}
    </div>
  )
}