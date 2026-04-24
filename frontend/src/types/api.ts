export interface SkillExtractionResult {
  jobTitle: string
  seniority: string
  requiredSkills: string[]
  niceToHaveSkills: string[]
  keywords: string[]
}

export interface ResumeMatchResult {
  matchScore: number
  matchedSkills: string[]
  missingSkills: string[]
  partialSkills: string[]
  summary: string
}

export interface RewrittenBullet {
  original: string
  rewritten: string
  reasoning: string
}

export interface ResumeRewriteResult {
  rewrittenBullets: RewrittenBullet[]
  reasoning: string
}

export interface CoverLetterResult {
  coverLetter: string
  strategyNotes: string
}

export interface PipelineResult {
  skillExtraction: SkillExtractionResult
  resumeMatch: ResumeMatchResult
  resumeRewrite: ResumeRewriteResult
  coverLetter: CoverLetterResult
}

export interface JobApplicationSummary {
  id: string
  status: string
  createdAt: string
  jobDescription: string
}

export interface JobApplicationDetail {
  id: string
  status: string
  createdAt: string
  jobDescription: string
  resumeText: string
  skillExtraction: SkillExtractionResult | null
  resumeMatch: ResumeMatchResult | null
  resumeRewrite: ResumeRewriteResult | null
  coverLetter: CoverLetterResult | null
}